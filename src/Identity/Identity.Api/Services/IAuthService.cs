using Core.Security.Dtos;
using Core.Security.JWT;

namespace Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<AccessToken> SignInAsync(UserSignInDto userSignIn, TimeSpan accessTokenExpireTime);
        Task SignUpAsync(UserSignUpDto userSignUp);
        Task<AccessToken> RefreshTokenSignInAsync(string refreshToken, TimeSpan accessTokenExpireTime);
    }
}
