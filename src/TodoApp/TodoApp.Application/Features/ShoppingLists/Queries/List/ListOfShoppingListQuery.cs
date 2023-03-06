using Core.Application.DynamicQuery;
using Core.Application.Requests;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Models;

namespace TodoApp.Application.Features.ShoppingLists.Queries.List
{
    public class ListOfShoppingListQuery : IRequest<ListOfShoppingListModel>
    {
        public Dynamic? Dynamic { get; set; }
        public PageRequest? PageRequest { get; set; }

        public class ListOfShoppingListQueryHandler : IRequestHandler<ListOfShoppingListQuery, ListOfShoppingListModel>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;

            public ListOfShoppingListQueryHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
            }

            public async Task<ListOfShoppingListModel> Handle(ListOfShoppingListQuery request, CancellationToken cancellationToken)
                => await _shoppingListService.GetAllShoppingListAsync(_httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"),
                    request.Dynamic ?? new(), request.PageRequest ?? new(), cancellationToken);
        }
    }
}
