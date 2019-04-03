using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App
{
    public class HttpRunner : BackgroundService
    {
        public HttpRunner(
            IApplicationLifetime applicationLifetime,
            IHttpClientFactory clientFactory,
            IOptions<HttpRunnerSettings> settings,
            ILogger<HttpRunner> logger)
        {
            ApplicationLifetime = applicationLifetime;
            ClientFactory = clientFactory;
            Logger = logger;
            Settings = settings.Value;
        }

        private IApplicationLifetime ApplicationLifetime { get; }
        private IHttpClientFactory ClientFactory { get; }
        private ILogger<HttpRunner> Logger { get; }
        private HttpRunnerSettings Settings { get; }

        private async Task RunTest(CancellationToken cancellationToken)
        {
            using (var client = ClientFactory.CreateClient(HttpRunnerSettings.HttpClientName))
            {
                client.DefaultRequestHeaders.ConnectionClose = Settings.ConnectionClose;
                client.Timeout = TimeSpan.FromSeconds(30);
                LogConfiguration(client);

                Logger.LogInformation("Test started");
                foreach (var delay in Settings.RequestIntervals.ToTimeSpans())
                {
                    await Delay(delay, cancellationToken);
                    await MakeRequest(client, cancellationToken);
                }

                Logger.LogInformation("Test done");

                if (Settings.ShutdownOnEnd)
                {
                    ApplicationLifetime.StopApplication();
                }
            }
        }

        private void LogConfiguration(HttpClient client)
        {
            Logger.LogInformation("Settings: {settings}", Settings);
            Logger.LogInformation("HttpClient.Timeout: {timeout}", client.Timeout);
        }

        private async Task Delay(TimeSpan? delay, CancellationToken cancellationToken)
        {
            if (delay == null || cancellationToken.IsCancellationRequested) return;

            Logger.LogInformation("About to sleep for {delay}", delay);
            await Task.Delay(delay.Value, cancellationToken);
            Logger.LogInformation("Resuming after sleep");
        }

        private async Task MakeRequest(HttpClient client, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var request = new HttpRequestMessage(HttpMethod.Get, Settings.Url);
            if (!string.IsNullOrWhiteSpace(Settings.Host))
            {
                request.Headers.Host = Settings.Host;
            }

            var response = await client.SendAsync(request, cancellationToken);
            var level = response.IsSuccessStatusCode ? LogLevel.Information : LogLevel.Warning;
            var content = await response.Content.ReadAsStringAsync();
            Logger.Log(level, "{response}{newline}{content}", response, Environment.NewLine, content);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunTest(stoppingToken);
        }
    }
}