using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using App.Builders;
using App.Examples;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sdk1Lib;

namespace App
{
    public static class Program
    {
        private const int Size = 1000;

        public static async Task Main()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "DEV";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.Configure<Settings>(configuration.GetSection(nameof(Settings)));

            services.AddSingleton<IExample, Example1>();
            services.AddSingleton<ISearchModelBuilder>(_ => new SearchModelBuilder(Size));
            services.AddSingleton(typeof(ISearchClient<>), typeof(SearchClient<>));

            services.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.DisableColors = false;
                    options.TimestampFormat = "[HH:mm:ss:fff] ";
                });
                builder.AddNonGenericLogger();
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();
            var examples = serviceProvider.GetServices<IExample>();

            var stopWatch = new Stopwatch();

            foreach (var example in examples)
            {
                stopWatch.Start();
                await example.RunAsync();
                stopWatch.Stop();

                logger.LogInformation("Example '{example}' took {duration}", example.Description, stopWatch.Elapsed.ToString("g"));
            }

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }

        private static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
        {
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                const string categoryName = nameof(Program);
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger(categoryName);
            });
        }
    }
}
