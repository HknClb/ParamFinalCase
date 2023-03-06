namespace Core.Security.JWT;

public class TokenOptions
{
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string SecurityKey { get; set; } = null!;
}