using Core.Security.Entities;
using Identity.Api.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Extensions
{
    public static class PersistenceServiceRegistrations
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("TodoDb") ?? throw new ArgumentNullException("Connection String");
            services.AddDbContext<TodoContext>(options => options.UseNpgsql(connectionString, options => options.MigrationsAssembly("TodoApp.Persistence")));
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

            return services;
        }
    }
}
