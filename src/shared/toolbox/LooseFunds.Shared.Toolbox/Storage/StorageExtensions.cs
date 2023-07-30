using Marten;
using Marten.Schema.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

namespace LooseFunds.Shared.Toolbox.Storage;

public static class StorageExtensions
{
    public static void AddStorage(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("Marten") ??
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
    }
}