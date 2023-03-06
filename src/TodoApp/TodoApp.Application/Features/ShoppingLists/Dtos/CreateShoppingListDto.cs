namespace TodoApp.Application.Features.ShoppingLists.Dtos
{
    public class CreateShoppingListDto
    {
        public string UserId { get; set; } = null!;
        public string ShoppingListCategoryId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
