namespace TodoApp.Application.Features.ShoppingLists.Dtos.Response
{
    public class ShoppingListGetByIdDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ShoppingListCategoryDto? Category { get; set; }
        public ICollection<ShoppingListItemDto>? Items { get; set; }
    }
}
