using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Security.Entities
{
    public class RefreshToken : Entity
    {
        public string UserId { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? Expires { get; set; }

        public virtual User User { get; set; } = null!;

        [NotMapped] public override Guid Id { get => base.Id; set => base.Id = value; }
        [NotMapped] public override bool IsActive { get => base.IsActive; set => base.IsActive = value; }
    }
}
