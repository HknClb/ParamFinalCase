using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;

namespace TodoApp.Application.Features.ShoppingLists.Queries.GetById
{
    public class GetShoppingListByIdQuery : IRequest<ShoppingListGetByIdDto>
    {
        public string ShoppingListId { get; set; } = null!;

        public class GetShoppingListByIdQueryHandler : IRequestHandler<GetShoppingListByIdQuery, ShoppingListGetByIdDto>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;

            public GetShoppingListByIdQueryHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
            }

            public async Task<ShoppingListGetByIdDto> Handle(GetShoppingListByIdQuery request, CancellationToken cancellationToken)
                 => await _shoppingListService.GetShoppingListByIdAsync(_httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"), request.ShoppingListId);
        }
    }
}
