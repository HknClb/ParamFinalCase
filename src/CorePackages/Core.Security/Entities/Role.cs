using Microsoft.AspNetCore.Identity;

namespace Core.Security.Entities
{
    public class Role : IdentityRole<string>
    {
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
