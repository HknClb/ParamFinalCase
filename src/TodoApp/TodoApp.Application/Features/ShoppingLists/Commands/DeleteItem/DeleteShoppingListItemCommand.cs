using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;

namespace TodoApp.Application.Features.ShoppingLists.Commands.DeleteItem
{
    public class DeleteShoppingListItemCommand : IRequest<ShoppingListItemDeletedDto>
    {
        public string ProductId { get; set; } = null!;
        public string ShoppingListId { get; set; } = null!;

        public class DeleteShoppingListItemCommandHandler : IRequestHandler<DeleteShoppingListItemCommand, ShoppingListItemDeletedDto>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;

            public DeleteShoppingListItemCommandHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
            }

            public async Task<ShoppingListItemDeletedDto> Handle(DeleteShoppingListItemCommand request, CancellationToken cancellationToken)
             => await _shoppingListService
                    .DeleteShoppingListItemAsync(_httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"), request.ProductId, request.ShoppingListId);
        }
    }
}
