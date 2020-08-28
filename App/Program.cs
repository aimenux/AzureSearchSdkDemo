using System;
using System.IO;
using System.Threading.Tasks;
using App.Builders;
using App.Examples;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App
{
    public static class Program
    {
        private const int Size = 100;

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
            services.AddSingleton<IExample, Example2>();
            services.AddSingleton<Sdk1Lib.SearchClient<Sdk1Lib.SearchIndex>>();
            services.AddSingleton<Sdk2Lib.SearchClient<Sdk2Lib.SearchIndex>>();
            services.AddSingleton<ISearchModelBuilder>(_ => new SearchModelBuilder(Size));

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole(options =>
                {
                    options.DisableColors = false;
                    options.TimestampFormat = "[HH:mm:ss:fff] ";
                });
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            });

            var serviceProvider = services.BuildServiceProvider();
            
            var builder = serviceProvider.GetService<ISearchModelBuilder>();
            var searchModels = builder.BuildSearchModels();
            var examples = serviceProvider.GetServices<IExample>();

            foreach (var example in examples)
            {
                await example.UploadAsync(searchModels);
                await example.CountAsync();
                await example.DeleteAllAsync();
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
