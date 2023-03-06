using Core.Application.DynamicQuery;
using Core.Application.Requests;
using TodoApp.Application.Features.ShoppingLists.Dtos;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;
using TodoApp.Application.Features.ShoppingLists.Models;

namespace TodoApp.Application.Abstractions.Services
{
    public interface IShoppingListService
    {
        public Task<ShoppingListGetByIdDto> GetShoppingListByIdAsync(string userId, string shoppingListId);
        public Task<ListOfShoppingListModel> GetAllShoppingListAsync(string userId, Dynamic dynamic, PageRequest pageRequest, CancellationToken cancellationToken = default);
        public Task<ShoppingListCreatedDto> CreateShoppingListAsync(CreateShoppingListDto createShoppingList);
        public Task<ShoppingListDeletedDto> DeleteShoppingListAsync(string userId, string shoppingListId);
        public Task<ShoppingListItemAddedDto> AddShoppingListItemAsync(AddShoppingListItemDto addShoppingListItem);
        public Task<ShoppingListItemUpdatedDto> UpdateShoppingListItemAsync(UpdateShoppingListItemDto updateShoppingListItem);
        public Task<ShoppingListItemDeletedDto> DeleteShoppingListItemAsync(string userId, string productId, string shoppingListId);
    }
}
