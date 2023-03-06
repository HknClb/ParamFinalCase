namespace Web.ApiGateway.Extensions
{
    public static class OcelotConfigurationExtension
    {
        public static IConfigurationBuilder ConfigureOcelot(this IConfigurationBuilder configuration)
            => configuration.AddConfiguration(new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("Configuration/ocelot.json")
                    .AddEnvironmentVariables()
                    .Build());
    }
}
