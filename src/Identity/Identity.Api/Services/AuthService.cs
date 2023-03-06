using Application.Abstractions.Services;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, ITokenHelper tokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public async Task<AccessToken> SignInAsync(UserSignInDto userSignIn, TimeSpan accessTokenExpireTime)
        {
            User? user = await _userManager.Users.Include(x => x.RefreshToken).Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.UserName == userSignIn.UserNameOrEmail || x.Email == userSignIn.UserNameOrEmail);
            if (user is null)
                throw new Exception("Invalid login attempt");
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user!, userSignIn.Password, false);
            if (!signInResult.Succeeded)
                throw new Exception("Invalid login attempt");

            AccessToken token = _tokenHelper.CreateToken(user, accessTokenExpireTime);

            RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, token);
            await _userService.UpdateRefreshTokenAsync(user, refreshToken);

            token.RefreshToken = refreshToken.Token!;

            return token;
        }

        public async Task<AccessToken> RefreshTokenSignInAsync(string refreshToken, TimeSpan accessTokenExpireTime)
        {
            User? user = await _userManager.Users.Include(x => x.RefreshToken).Include(x => x.Roles).FirstOrDefaultAsync(x => x.RefreshToken.Token == refreshToken);
            if (user is null)
                throw new Exception("Invalid login attempt");
            if (user.RefreshToken.Expires > DateTime.UtcNow)
                throw new Exception("Access token expired");

            AccessToken token = _tokenHelper.CreateToken(user, accessTokenExpireTime);
            return token;
        }

        public async Task SignUpAsync(UserSignUpDto userSignUp)
        {
            User user = new(userSignUp.Email, new());
            IdentityResult result = await _userManager.CreateAsync(user, userSignUp.Password);
            if (!result.Succeeded)
            {
                string errorMessage = string.Empty;
                foreach (var error in result.Errors)
                    errorMessage += $"[{error.Code}] {error.Description}";
                throw new Exception(errorMessage);
            }
        }
    }
}