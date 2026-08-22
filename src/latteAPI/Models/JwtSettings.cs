namespace latteAPI.Models;

public class JwtSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigningKey { get; init; }
    public required int ExpiryHours { get; init; }
}
