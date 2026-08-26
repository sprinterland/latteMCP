#!/usr/bin/env python3
"""Safe append-only JSONL helper.

Used for any append-only ".jsonl" log this project's skills write to (a Task Record's
status.jsonl, and any other project-side JSONL log added later) -- never rewrites or deletes an
existing line, and generates ids in the same "<PREFIX>-<epoch-ms>-<hex4>" no-coordination-needed
scheme used by _frw's own change_requests.jsonl (see docs/framework-maintenance.md).

Usage:
  append_jsonl.py --gen-id PREFIX
      Print a new "<PREFIX>-<epoch-ms>-<hex4>" id and exit. Touches no file.

  append_jsonl.py <path> --gen-sequential-id PREFIX
      Scan <path> for existing "<PREFIX>-<digits>" ids (each line's own "id" field) and print one
      past the highest, zero-padded to at least 4 digits (e.g. "REV-0016"). Touches no file. For
      push_reviews.jsonl/update_history.jsonl's one-at-a-time REV-NNNN/UPD-NNNN counters only --
      change_requests.jsonl keeps using --gen-id, since its many concurrent, uncoordinated writers
      make a scan-and-increment scheme unsafe there.

  append_jsonl.py <path> --json '<json object>'
  append_jsonl.py <path> --stdin
      Validate the given value as a single JSON object (not an array or scalar) and append it as
      one line to <path>, creating the file itself if it doesn't exist yet (the parent directory
      must already exist -- this script never creates directories).

Exits non-zero with a message on invalid input or a missing parent directory; a single os.write()
per call means one append is atomic with respect to any other concurrent append to the same file
on a local filesystem -- never partially or interleaved.
"""
import argparse
import json
import os
import random
import sys
import time


def gen_id(prefix: str) -> str:
    epoch_ms = int(time.time() * 1000)
    hexpart = "".join(random.choice("0123456789abcdef") for _ in range(4))
    return f"{prefix}-{epoch_ms}-{hexpart}"


def gen_sequential_id(prefix: str, path: str) -> str:
    # Parse each line as the JSON object append_line() guarantees it is, the same way the rest of
    # this file validates input, rather than pattern-matching the raw text -- a line-oriented regex
    # would have to special-case incidental formatting and could false-match a free-text field that
    # happens to contain the literal substring being searched for.
    max_n = 0
    try:
        f = open(path, "r", encoding="utf-8")
    except FileNotFoundError:
        f = None
    if f is not None:
        with f:
            for line in f:
                try:
                    obj = json.loads(line)
                except json.JSONDecodeError:
                    continue
                if not isinstance(obj, dict):
                    continue
                id_value = obj.get("id", "")
                if isinstance(id_value, str) and id_value.startswith(prefix + "-"):
                    suffix = id_value[len(prefix) + 1:]
                    # str.isdigit() accepts non-ASCII Unicode digits (e.g. superscripts) that
                    # int() can't parse -- ASCII-restrict first so a corrupted id can't crash this.
                    if suffix.isascii() and suffix.isdigit():
                        max_n = max(max_n, int(suffix))
    return f"{prefix}-{max_n + 1:04d}"


def append_line(path: str, obj: dict) -> None:
    data = (json.dumps(obj, ensure_ascii=False) + "\n").encode("utf-8")
    # A single os.write() to an O_APPEND-opened fd, not a buffered text-mode file object -- a
    # buffered writer can split one line across more than one underlying write() syscall even for
    # a small dict, and two concurrent appenders' writes can then interleave mid-line. A single
    # write() of the complete line relies on the well-established local-filesystem guarantee that
    # one write() to an O_APPEND fd is atomic with respect to other appenders' write() calls.
    fd = os.open(path, os.O_CREAT | os.O_APPEND | os.O_WRONLY)
    try:
        os.write(fd, data)
    finally:
        os.close(fd)


def main() -> int:
    parser = argparse.ArgumentParser(description="Safe JSONL id-generation and append helper.")
    parser.add_argument("path", nargs="?", help="Path to the .jsonl file to append to")
    parser.add_argument("--gen-id", metavar="PREFIX", help="Print a new <PREFIX>-<epoch-ms>-<hex4> id and exit")
    parser.add_argument(
        "--gen-sequential-id",
        metavar="PREFIX",
        help="Scan <path> for existing <PREFIX>-NNNN ids and print the next one; requires path",
    )
    parser.add_argument("--json", metavar="JSON", help="A single JSON object to append")
    parser.add_argument("--stdin", action="store_true", help="Read the JSON object to append from stdin")
    args = parser.parse_args()

    if args.gen_id is not None:
        print(gen_id(args.gen_id))
        return 0

    if args.gen_sequential_id is not None:
        if not args.path:
            parser.error("path is required with --gen-sequential-id")
        print(gen_sequential_id(args.gen_sequential_id, args.path))
        return 0

    if not args.path:
        parser.error("path is required unless --gen-id is used")

    if args.stdin:
        raw = sys.stdin.read()
    elif args.json is not None:
        raw = args.json
    else:
        parser.error("one of --json or --stdin is required unless --gen-id is used")

    try:
        obj = json.loads(raw)
    except json.JSONDecodeError as e:
        print(f"error: invalid JSON: {e}", file=sys.stderr)
        return 1

    if not isinstance(obj, dict):
        print("error: the value to append must be a single JSON object, not an array or scalar", file=sys.stderr)
        return 1

    try:
        append_line(args.path, obj)
    except FileNotFoundError:
        print(
            f"error: parent directory does not exist for {args.path} -- this script creates the "
            "file, not its containing directory",
            file=sys.stderr,
        )
        return 1
    print(f"appended 1 line to {args.path}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
