using Core.Security.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Abstractions.UnitOfWorks;
using TodoApp.Persistence.Contexts;
using TodoApp.Persistence.Services;
using TodoApp.Persistence.UnitOfWorks;

namespace TodoApp.Persistence
{
    public static class PersistenceServiceRegistrations
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("TodoDb") ?? throw new ArgumentNullException("Connection String");
            services.AddDbContext<TodoContext>(options =>
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseNpgsql(connectionString, options => options.MigrationsAssembly("TodoApp.Persistence"));
            });
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;

            }).AddEntityFrameworkStores<TodoContext>()
              .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IShoppingListService, ShoppingListService>();

            return services;
        }
    }
}
