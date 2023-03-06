using Core.Application.Paging;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;

namespace TodoApp.Application.Features.ShoppingLists.Models
{
    public class ListOfShoppingListModel : BasePageableModel
    {
        public ICollection<ListOfShoppingListDto> Items { get; set; } = null!;
    }
}
