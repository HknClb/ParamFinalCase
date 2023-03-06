namespace TodoApp.Application.Features.ShoppingLists.Dtos
{
    public class ShoppingListItemDto
    {
        public ShoppingListProductDto? Product { get; set; }
        public int? Quantity { get; set; }
        public string? Measurement { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
