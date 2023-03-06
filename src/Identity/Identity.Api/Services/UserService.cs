using Core.Security.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateRefreshTokenAsync(User user, RefreshToken refreshToken)
        {
            user.RefreshToken.Token = refreshToken.Token;
            user.RefreshToken.Expires = refreshToken.Expires;
            IdentityResult result = await _userManager.UpdateAsync(user);
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