using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;

namespace TodoApp.Application.Features.ShoppingLists.Commands.Delete
{
    public class DeleteShoppingListCommand : IRequest<ShoppingListDeletedDto>
    {
        public string ShoppingListId { get; set; } = null!;

        public class DeleteShoppingListCommandHandler : IRequestHandler<DeleteShoppingListCommand, ShoppingListDeletedDto>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;

            public DeleteShoppingListCommandHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
            }

            public async Task<ShoppingListDeletedDto> Handle(DeleteShoppingListCommand request, CancellationToken cancellationToken)
                => await _shoppingListService.DeleteShoppingListAsync(_httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"), request.ShoppingListId);
        }
    }
}
