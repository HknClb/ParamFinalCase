using Microsoft.AspNetCore.Identity;

namespace Core.Security.Entities
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User(string email, RefreshToken refreshToken, string? userName = null, string? phoneNumber = null) : base(userName ?? email)
        {
            Id = Guid.NewGuid().ToString();
            Email = email;
            RefreshToken = refreshToken;
        }

        public virtual RefreshToken RefreshToken { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
