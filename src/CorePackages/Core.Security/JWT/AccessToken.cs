namespace Core.Security.JWT;

public class AccessToken
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}