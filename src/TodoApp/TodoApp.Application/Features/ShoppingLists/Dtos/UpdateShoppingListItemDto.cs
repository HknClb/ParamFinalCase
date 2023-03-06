using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.ShoppingLists.Dtos
{
    public class UpdateShoppingListItemDto
    {
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string ShoppingListId { get; set; } = null!;
        public int? Quantity { get; set; }
        public MeasurementType? MeasurementType { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
