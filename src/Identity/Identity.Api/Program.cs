using Application;
using Application.Abstractions.Services;
using Core.Security.Dtos;
using FluentValidation;
using Identity.Api.Extensions;
using Identity.Api.Services;
using Identity.Api.Validators;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddSecurityServices();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<UserSignUpDto>, SignUpValidator>();
builder.Services.AddScoped<IValidator<UserSignInDto>, SignInValidator>();

builder.Services.ConfigureConsul(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.RegisterConsul(app.Lifetime, app.Configuration);

app.Run();
