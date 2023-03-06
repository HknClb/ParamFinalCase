using AutoMapper;
using Core.Application.Paging;
using TodoApp.Application.Features.ShoppingLists.Commands.AddItem;
using TodoApp.Application.Features.ShoppingLists.Commands.Create;
using TodoApp.Application.Features.ShoppingLists.Commands.UpdateItem;
using TodoApp.Application.Features.ShoppingLists.Dtos;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;
using TodoApp.Application.Features.ShoppingLists.Models;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.ShoppingLists.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ShoppingListProductDto>().ReverseMap();
            CreateMap<ShoppingListItem, ShoppingListItemDto>()
                .ForMember(dest => dest.Measurement, opt => opt.MapFrom(src => src.MeasurementType.ToString()))
                .ReverseMap();
            CreateMap<ShoppingListCategory, ShoppingListCategoryDto>().ReverseMap();

            CreateMap<ShoppingList, ShoppingListGetByIdDto>().ReverseMap();

            CreateMap<ShoppingList, ListOfShoppingListDto>().ReverseMap();
            CreateMap<IPaginate<ShoppingList>, ListOfShoppingListModel>().ReverseMap();

            CreateMap<CreateShoppingListCommand, CreateShoppingListDto>().ReverseMap();
            CreateMap<CreateShoppingListDto, ShoppingList>().ReverseMap();
            CreateMap<ShoppingList, ShoppingListCreatedDto>().ReverseMap();

            CreateMap<AddShoppingListItemCommand, AddShoppingListItemDto>().ReverseMap();
            CreateMap<AddShoppingListItemDto, ShoppingListItem>().ReverseMap();
            CreateMap<ShoppingListItem, ShoppingListItemAddedDto>().ReverseMap();

            CreateMap<UpdateShoppingListItemCommand, UpdateShoppingListItemDto>().ReverseMap();
            CreateMap<UpdateShoppingListItemDto, ShoppingListItem>().ReverseMap();
            CreateMap<ShoppingListItem, ShoppingListItemUpdatedDto>().ReverseMap();
        }
    }
}
