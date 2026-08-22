using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol;

namespace latteMCP;

// latteAPI serializes enums as their string name, not the underlying integer (see
// ../../../docs/api-conventions.md). Neither System.Net.Http.Json's own defaults nor
// McpJsonUtilities.DefaultOptions include that converter, so both the outbound HttpClient calls
// to latteAPI and the MCP tool schema/argument (de)serialization need it applied explicitly to
// stay wire-compatible with latteAPI and with each other. Copied from McpJsonUtilities.DefaultOptions
// rather than built from scratch so the reflection-based tool binder still gets a populated
// TypeInfoResolver chain (it throws otherwise).
public static class LatteApiJsonOptions
{
    public static readonly JsonSerializerOptions Default = new(McpJsonUtilities.DefaultOptions)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
