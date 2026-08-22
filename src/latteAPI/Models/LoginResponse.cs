namespace latteAPI.Models;

public record LoginResponse(string Token, DateTimeOffset ExpiresAt);
