using AutoMapper;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Dtos;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.ShoppingLists.Commands.UpdateItem
{
    public class UpdateShoppingListItemCommand : IRequest<ShoppingListItemUpdatedDto>
    {
        public string ProductId { get; set; } = null!;
        public string ShoppingListId { get; set; } = null!;
        public int? Quantity { get; set; }
        public MeasurementType? MeasurementType { get; set; }
        public bool? IsCompleted { get; set; }

        public class UpdateShoppingListItemCommandHandler : IRequestHandler<UpdateShoppingListItemCommand, ShoppingListItemUpdatedDto>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;
            private readonly IMapper _mapper;

            public UpdateShoppingListItemCommandHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor, IMapper mapper)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
                _mapper = mapper;
            }

            public async Task<ShoppingListItemUpdatedDto> Handle(UpdateShoppingListItemCommand request, CancellationToken cancellationToken)
            {
                UpdateShoppingListItemDto updateShoppingListItem = new()
                {
                    UserId = _httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"),
                };
                return await _shoppingListService.UpdateShoppingListItemAsync(_mapper.Map(request, updateShoppingListItem));
            }
        }
    }
}
