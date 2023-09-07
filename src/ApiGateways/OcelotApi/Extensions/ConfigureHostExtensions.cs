namespace OcelotApi.Extensions
{
    public static class ConfigureHostExtensions
    {
        public static void AddAppConfigurations(this ConfigureHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
        }
    }
}
