namespace latteMCP.Models;

// latteAPI's POST /auth/login response shape, returned as-is (MCP-REQ-004).
public record LoginResponse(string Token, DateTimeOffset ExpiresAt);
