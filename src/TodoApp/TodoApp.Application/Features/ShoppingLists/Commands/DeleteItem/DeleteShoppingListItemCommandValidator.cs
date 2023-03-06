using FluentValidation;

namespace TodoApp.Application.Features.ShoppingLists.Commands.DeleteItem
{
    public class DeleteShoppingListItemCommandValidator : AbstractValidator<DeleteShoppingListItemCommand>
    {
        public DeleteShoppingListItemCommandValidator()
        {

        }
    }
}
