using Core.Security.Entities;

namespace Core.Security.JWT;

public interface ITokenHelper
{
    AccessToken CreateToken(User user, TimeSpan accessTokenExpiration);

    RefreshToken CreateRefreshToken(User user, AccessToken accessToken);
}