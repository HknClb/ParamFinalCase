using Core.Security.Extensions;
using TodoApp.Application;
using TodoApp.Persistence;
using TodoApp.ReadApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureConsul(builder.Configuration);

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddApplicationServices()
    .AddAuthServices(builder.Configuration);

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
