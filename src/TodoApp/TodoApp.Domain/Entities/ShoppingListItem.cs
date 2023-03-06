using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities
{
    public class ShoppingListItem : Entity
    {
        public Guid ProductId { get; set; }
        public Guid ShoppingListId { get; set; }

        public int Quantity { get; set; }
        public MeasurementType MeasurementType { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual ShoppingList ShoppingList { get; set; } = null!;

        [NotMapped] public override Guid Id { get => base.Id; set => base.Id = value; }
    }
}
