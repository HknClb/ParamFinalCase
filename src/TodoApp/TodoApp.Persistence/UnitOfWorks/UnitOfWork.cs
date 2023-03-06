using Core.Persistence.UnitOfWorks.Base;
using TodoApp.Application.Abstractions.UnitOfWorks;
using TodoApp.Persistence.Contexts;

namespace TodoApp.Persistence.UnitOfWorks
{
    public class UnitOfWork : EfUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(TodoContext todoContext) : base(todoContext)
        {
        }
    }
}
