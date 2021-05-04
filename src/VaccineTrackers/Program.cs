using ConsoleAppFramework;
using Cowin.Domain.Abstractions;
using Cowin.Infrastructure.Files;
using Cowin.Infrastructure.Notification;
using Cowin.Infrastructure.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cowin.VaccineTrackers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => {
                    config.AddUserSecrets<Program>(true);
                })
                .ConfigureLogging((context, logging) => {
                    logging.AddSerilog(FileLoggerConfig().CreateLogger());
                })
                .ConfigureServices((context, services) => {
                    services.AddSingleton<HttpClient>(provider => {
                        var httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri("https://cdn-api.co-vin.in/api/v2/");
                        httpClient.Timeout = new TimeSpan(0, 0, 60);
                        httpClient.DefaultRequestHeaders.Clear();

                        return httpClient;
                    });

                    services.AddSingleton<ICowinHttpClient, CowinHttpClient>();
                    services.AddSingleton<INotificationService, ConsoleNotification>();
                })
                .RunConsoleAppFrameworkAsync(args);
        }

        static LoggerConfiguration FileLoggerConfig()
        {
            var logfile = FileHelpers.CreateLogFile();
            return new LoggerConfiguration()
                .WriteTo.File(
                    logfile,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 104857600, // 100MB
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 60);
        }
    }
}
