using Core.Domain.Entities;
using Core.Security.Entities;

namespace TodoApp.Domain.Entities
{
    public class ShoppingList : Entity
    {
        public string UserId { get; set; } = null!;
        public Guid ShoppingListCategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ShoppingListCategory Category { get; set; } = null!;
        public virtual ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
    }
}