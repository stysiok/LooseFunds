using LooseFunds.Shared.Toolbox.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static void AddUnitOfWork(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
        => services
            .AddStorage(configuration, environment)    
            .AddScoped<IUnitOfWork, UnitOfWork>();
}