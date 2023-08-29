using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LooseFunds.Shared.Toolbox.Configuration;

public static class ConfigurationExtensions
{
    public static void AddConfigurations(this ConfigurationManager configurationManager, IHostEnvironment environment)
    {
        configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();
    }
}