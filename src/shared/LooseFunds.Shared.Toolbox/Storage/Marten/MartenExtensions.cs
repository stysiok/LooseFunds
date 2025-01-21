using Marten;
using Marten.Schema.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

namespace LooseFunds.Shared.Toolbox.Storage.Marten;

public static class MartenExtensions
{
    public static IServiceCollection AddMartenStorage(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        string connectionString = configuration.GetConnectionString("Marten") ??
                                  throw new InvalidOperationException("Missing marten connection string");

        services.AddMarten(options =>
        {
            options.Connection(connectionString);

            options.Policies.ForAllDocuments(dm =>
            {
                if (dm.IdType == typeof(Guid)) dm.IdStrategy = new CombGuidIdGeneration();
            });

            if (environment.IsDevelopment()) options.AutoCreateSchemaObjects = AutoCreate.All;
        });

        services.AddScoped<IStorage, MartenStorage>();

        return services;
    }
}
