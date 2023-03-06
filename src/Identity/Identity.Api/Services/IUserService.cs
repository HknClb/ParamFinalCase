using Core.Security.Entities;

namespace Identity.Api.Services
{
    public interface IUserService
    {
        Task UpdateRefreshTokenAsync(User user, RefreshToken refreshToken);
    }
}
