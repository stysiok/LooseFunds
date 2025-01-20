using LooseFunds.Shared.Toolbox.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Investor.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services.AddSingleton<IEventsMapper, InvestorEventsMapper>();
}