using Core.Domain.Entities;

namespace TodoApp.Domain.Entities
{
    public class Product : Entity
    {
        public Product()
        {
        }

        public Product(string name, string? description = null) : this()
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}