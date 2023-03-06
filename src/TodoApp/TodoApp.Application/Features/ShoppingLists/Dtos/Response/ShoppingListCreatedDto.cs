namespace TodoApp.Application.Features.ShoppingLists.Dtos.Response
{
    public class ShoppingListCreatedDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ShoppingListCategoryDto? Category { get; set; }
    }
}
