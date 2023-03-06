using Core.Application.DynamicQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Features.ShoppingLists.Queries.GetById;
using TodoApp.Application.Features.ShoppingLists.Queries.List;
using TodoApp.ReadApi.Controllers.Base;

namespace TodoApp.ReadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListsController : BaseController
    {
        [HttpGet]
        [DynamicQuery]
        public async Task<IActionResult> ListAsync([FromQuery] ListOfShoppingListQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{ShoppingListId}")]
        public async Task<IActionResult> GetAsync([FromRoute] GetShoppingListByIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
