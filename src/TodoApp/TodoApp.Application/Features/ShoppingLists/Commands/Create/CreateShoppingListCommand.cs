using AutoMapper;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Features.ShoppingLists.Dtos;
using TodoApp.Application.Features.ShoppingLists.Dtos.Response;

namespace TodoApp.Application.Features.ShoppingLists.Commands.Create
{
    public class CreateShoppingListCommand : IRequest<ShoppingListCreatedDto>
    {
        public string ShoppingListCategoryId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public class CreateShoppingListCommandHandler : IRequestHandler<CreateShoppingListCommand, ShoppingListCreatedDto>
        {
            private readonly IShoppingListService _shoppingListService;
            private readonly HttpContext _httpContext;
            private readonly IMapper _mapper;

            public CreateShoppingListCommandHandler(IShoppingListService shoppingListService, IHttpContextAccessor contextAccessor, IMapper mapper)
            {
                _shoppingListService = shoppingListService;
                _httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException("Only http requests are supported.");
                _mapper = mapper;
            }

            public async Task<ShoppingListCreatedDto> Handle(CreateShoppingListCommand request, CancellationToken cancellationToken)
            {
                CreateShoppingListDto createShoppingList = new()
                {
                    UserId = _httpContext.User.GetUserId() ?? throw new ArgumentNullException("UserId"),
                };
                return await _shoppingListService.CreateShoppingListAsync(_mapper.Map(request, createShoppingList));
            }
        }
    }
}
