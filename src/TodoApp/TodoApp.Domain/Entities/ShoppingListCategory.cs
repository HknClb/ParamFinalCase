using Core.Domain.Entities;

namespace TodoApp.Domain.Entities
{
    public class ShoppingListCategory : Entity
    {
        public ShoppingListCategory()
        {
        }

        public ShoppingListCategory(string name, string? description = null) : this()
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
    }
}