using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Features.ShoppingLists.Commands.AddItem;
using TodoApp.Application.Features.ShoppingLists.Commands.Create;
using TodoApp.Application.Features.ShoppingLists.Commands.Delete;
using TodoApp.Application.Features.ShoppingLists.Commands.DeleteItem;
using TodoApp.Application.Features.ShoppingLists.Commands.UpdateItem;
using TodoApp.WriteApi.Controllers.Base;

namespace TodoApp.WriteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateShoppingListCommand command)
        {
            return Created("", await Mediator.Send(command));
        }

        [HttpDelete("{ShoppingListId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteShoppingListCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddItemAsync([FromBody] AddShoppingListItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] UpdateShoppingListItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("[action]/{ShoppingListId}")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute] string ShoppingListId, [FromQuery] string ProductId)
        {
            DeleteShoppingListItemCommand command = new()
            {
                ShoppingListId = ShoppingListId,
                ProductId = ProductId
            };
            return Ok(await Mediator.Send(command));
        }
    }
}
