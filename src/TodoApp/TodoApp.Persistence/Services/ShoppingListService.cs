using AutoMapper;
using Core.Application.DynamicQuery;
using Core.Application.Paging;
using Core.Application.Requests;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Abstractions.UnitOfWorks;
using TodoApp.Application.Features.ShoppingLists.Dtos;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;
using TodoApp.Application.Features.ShoppingLists.Models;
using TodoApp.Application.Features.ShoppingLists.Rules;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingListBusinessRules _shoppingListBusinessRules;
        private readonly IMapper _mapper;

        public ShoppingListService(IUnitOfWork unitOfWork, ShoppingListBusinessRules shoppingListBusinessRules, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _shoppingListBusinessRules = shoppingListBusinessRules;
            _mapper = mapper;
        }

        public async Task<ShoppingListGetByIdDto> GetShoppingListByIdAsync(string userId, string shoppingListId)
        {
            ShoppingList? shoppingList = await _unitOfWork.ReadRepository<ShoppingList>().GetAsync(
                x => x.UserId == userId && x.Id == Guid.Parse(shoppingListId) && x.IsActive,
                x => x.Include(x => x.Category).Include(x => x.Items).ThenInclude(item => item.Product), false);
            _shoppingListBusinessRules.ShoppingListShouldBeExist(shoppingList);
            return _mapper.Map<ShoppingListGetByIdDto>(shoppingList);
        }

        public async Task<ListOfShoppingListModel> GetAllShoppingListAsync(string userId, Dynamic dynamic, PageRequest pageRequest, CancellationToken cancellationToken = default)
        {
            IPaginate<ShoppingList> paginate = await _unitOfWork.ReadRepository<ShoppingList>().GetListByDynamicAsPaginateAsync(dynamic,
                x => x.UserId == userId, x => x.Include(x => x.Category).Include(x => x.Items).ThenInclude(item => item.Product),
                pageRequest.Page, pageRequest.PageSize, false, cancellationToken);
            return _mapper.Map<ListOfShoppingListModel>(paginate);
        }

        public async Task<ShoppingListCreatedDto> CreateShoppingListAsync(CreateShoppingListDto createShoppingList)
        {
            ShoppingList shoppingList = await _unitOfWork.WriteRepository<ShoppingList>().AddAsync(_mapper.Map<ShoppingList>(createShoppingList));
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ShoppingListCreatedDto>(shoppingList);
        }

        public async Task<ShoppingListDeletedDto> DeleteShoppingListAsync(string userId, string shoppingListId)
        {
            ShoppingList? shoppingList = await _unitOfWork.ReadRepository<ShoppingList>().GetAsync(x => x.UserId == userId && x.Id == Guid.Parse(shoppingListId));
            _shoppingListBusinessRules.ShoppingListShouldBeExist(shoppingList);
            await _unitOfWork.WriteRepository<ShoppingList>().DeleteAsync(shoppingList!);
            await _unitOfWork.CompleteAsync();
            return new();
        }

        public async Task<ShoppingListItemAddedDto> AddShoppingListItemAsync(AddShoppingListItemDto addShoppingListItem)
        {
            ShoppingList? shoppingList = await _unitOfWork.ReadRepository<ShoppingList>().GetAsync(
               x => x.UserId == addShoppingListItem.UserId && x.Id == Guid.Parse(addShoppingListItem.ShoppingListId) && x.IsActive,
               x => x.Include(x => x.Items));
            _shoppingListBusinessRules.ShoppingListShouldBeExist(shoppingList);
            shoppingList!.Items.Add(_mapper.Map<ShoppingListItem>(addShoppingListItem));
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ShoppingListItemAddedDto>(shoppingList.Items.First(x => x.ProductId == Guid.Parse(addShoppingListItem.ProductId)));
        }

        public async Task<ShoppingListItemUpdatedDto> UpdateShoppingListItemAsync(UpdateShoppingListItemDto updateShoppingListItem)
        {
            ShoppingList? shoppingList = await _unitOfWork.ReadRepository<ShoppingList>().GetAsync(
              x => x.UserId == updateShoppingListItem.UserId && x.Id == Guid.Parse(updateShoppingListItem.ShoppingListId) && x.IsActive,
              x => x.Include(x => x.Items));
            _shoppingListBusinessRules.ShoppingListShouldBeExist(shoppingList);
            ShoppingListItem shoppingListItem = shoppingList!.Items.First(x => x.ProductId == Guid.Parse(updateShoppingListItem.ProductId));
            shoppingListItem = _mapper.Map(updateShoppingListItem, shoppingListItem);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ShoppingListItemUpdatedDto>(shoppingListItem);
        }

        public async Task<ShoppingListItemDeletedDto> DeleteShoppingListItemAsync(string userId, string productId, string shoppingListId)
        {
            ShoppingList? shoppingList = await _unitOfWork.ReadRepository<ShoppingList>().GetAsync(
              x => x.UserId == userId && x.Id == Guid.Parse(shoppingListId) && x.IsActive,
              x => x.Include(x => x.Items));
            _shoppingListBusinessRules.ShoppingListShouldBeExist(shoppingList);
            shoppingList!.Items.Remove(shoppingList!.Items.First(x => x.ProductId == Guid.Parse(productId)));
            await _unitOfWork.CompleteAsync();
            return new();
        }
    }
}