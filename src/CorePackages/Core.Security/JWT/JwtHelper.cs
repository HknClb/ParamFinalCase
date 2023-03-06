using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Core.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private readonly TokenOptions _tokenOptions;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetRequiredSection("Security:JwtBearer").Get<TokenOptions>() ?? throw new ArgumentNullException(nameof(TokenOptions));
    }

    public AccessToken CreateToken(User user, TimeSpan accessTokenExpiration)
    {
        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, accessTokenExpiration);
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string? token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken
        {
            Token = token,
            Expiration = DateTime.UtcNow.Add(accessTokenExpiration)
        };
    }

    public RefreshToken CreateRefreshToken(User user, AccessToken accessToken)
    {
        RefreshToken refreshToken = new()
        {
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = accessToken.Expiration.AddDays(7)
        };

        return refreshToken;
    }

    public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, TimeSpan accessTokenExpiration)
    {
        JwtSecurityToken jwt = new(
            tokenOptions.Issuer,
            tokenOptions.Audience,
            expires: DateTime.UtcNow.Add(accessTokenExpiration),
            notBefore: DateTime.Now,
            claims: SetClaims(user),
            signingCredentials: signingCredentials
        );
        return jwt;
    }

    private static IEnumerable<Claim> SetClaims(User user)
    {
        List<Claim> claims = new();
        claims.AddNameIdentifier(user.Id);
        claims.AddEmail(user.Email!);
        claims.AddName(user.UserName!);
        claims.AddRoles(user.Roles.Where(c => c.Name is not null).Select(c => c.Name!)?.ToArray() ?? Array.Empty<string>());
        return claims;
    }
}