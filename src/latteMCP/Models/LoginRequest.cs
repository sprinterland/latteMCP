namespace latteMCP.Models;

// Forwarded to latteAPI's POST /auth/login unchanged (MCP-REQ-004).
public record LoginRequest(string Username, string Password);
