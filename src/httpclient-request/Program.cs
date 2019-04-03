using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile("appsettings-overrides.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var settingsSection = hostContext.Configuration.GetSection("HttpRunner");
                    services
                        .AddHttpClient(HttpRunnerSettings.HttpClientName)
                        .SetHandlerLifetime(settingsSection.Get<HttpRunnerSettings>().PooledConnectionIdleTimeout);

                    services.Configure<HttpRunnerSettings>(settingsSection);
                    services.AddHostedService<HttpRunner>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }
}