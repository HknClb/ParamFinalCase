using Consul;

namespace TodoApp.WriteApi.Extensions
{
    public static class ConsulRegistration
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new(config =>
            {
                string address = configuration["ConsulConfig:Address"] ?? throw new ArgumentNullException("Consul Address");
                config.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            // Get ConsuleClient that we registered above from ServiceProvider
            IConsulClient consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            // Server IP Address
            Uri uri = new(configuration["ServerConfig:Address"] ?? throw new ArgumentNullException("Server Address"));

            // Create instance of ServiceRegistrations
            AgentServiceRegistration registration = new()
            {
                ID = "TodoAppWriteService",
                Name = "TodoAppWriteService",
                Address = uri.Host,
                Port = uri.Port,
                Tags = new[] { "TodoAppWriteService", "TodoAppWrite", "TodoApp" }
            };

            consulClient.Agent.ServiceDeregister(registration.ID).Wait(); // Deregister registration if already exist.
            consulClient.Agent.ServiceRegister(registration).Wait(); // Register registration.

            lifetime.ApplicationStopping.Register(() => consulClient.Agent.ServiceDeregister(registration.ID).Wait()); // Deregister service when application stopping.

            return app;
        }
    }
}