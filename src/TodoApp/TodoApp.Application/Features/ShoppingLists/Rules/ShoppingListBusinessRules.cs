using Core.CrossCuttingConcerns.Exceptions.Business;
using Core.Security.Entities;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.ShoppingLists.Rules
{
    public class ShoppingListBusinessRules
    {
        public void UserShouldBeExist(User? user)
        {
            if (user is null)
                throw new BusinessException("The user couldn't found");
        }

        public void ShoppingListShouldBeExist(ShoppingList? shoppingList)
        {
            if (shoppingList is null)
                throw new BusinessException("Shopping List not exist");
        }
    }
}
