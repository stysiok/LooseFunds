using System.Reflection;
using LooseFunds.Shared.Toolbox.Correlation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LooseFunds.Shared.Toolbox.Logging;

public static class LoggingServiceCollectionExtensions
{
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        => hostBuilder.UseSerilog((context, serviceProvider, loggerConfiguration) =>
        {
            var loggingOptions = configuration.GetSection(LoggingConst.LoggingSection).Get<LoggingOptions>();
            var applicationName =
                Assembly.GetEntryAssembly()?.FullName?.Split(',')[0].ToLowerInvariant() ?? string.Empty;

            if (context.HostingEnvironment.IsDevelopment())
                loggerConfiguration.MinimumLevel.Debug();
            else
                loggerConfiguration.MinimumLevel.Information();
            
            var enricher = serviceProvider.GetService<CorrelationLogEnricher>();
            if (enricher is not null)
                loggerConfiguration.Enrich.With(enricher);
            
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .WriteTo.Console();

            if (!string.IsNullOrWhiteSpace(loggingOptions?.Seq)) 
                loggerConfiguration.WriteTo.Seq(loggingOptions.Seq);
        });
}

