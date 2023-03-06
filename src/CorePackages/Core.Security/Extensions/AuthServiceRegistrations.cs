using Core.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Core.Security.Extensions
{
    public static class AuthServiceRegistrations
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            TokenOptions tokenOptions = configuration.GetRequiredSection("Security:JwtBearer").Get<TokenOptions>() ?? throw new ArgumentNullException(nameof(TokenOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),

                    ValidateIssuer = true,
                    ValidIssuer = tokenOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = tokenOptions.Audience,

                    ValidateLifetime = true,
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires is not null && expires > DateTime.UtcNow,
                    ClockSkew = TimeSpan.Zero,

                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
            });

            return services;
        }
    }
}
