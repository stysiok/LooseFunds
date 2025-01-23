using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, UnitOfWork>();

    public static IServiceCollection AddDomainUnitOfWork(this IServiceCollection services)
        => services.AddScoped<IUnitOfWork, DomainUnitOfWork>();
}
