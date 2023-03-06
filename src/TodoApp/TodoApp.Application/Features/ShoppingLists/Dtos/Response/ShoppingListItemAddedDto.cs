namespace TodoApp.Application.Features.ShoppingLists.Dtos.Response
{
    public class ShoppingListItemAddedDto
    {
        public ShoppingListProductDto? Product { get; set; }
        public int? Quantity { get; set; }
        public string? Measurement { get; set; }
    }
}
