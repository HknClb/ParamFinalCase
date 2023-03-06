using Core.Security.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthServices(builder.Configuration);

builder.Configuration.ConfigureOcelot();

builder.Services.AddOcelot().AddConsul();

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

await app.UseOcelot();

app.Run();
