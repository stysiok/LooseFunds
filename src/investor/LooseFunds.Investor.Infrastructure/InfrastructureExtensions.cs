using LooseFunds.Investor.Core.Domain;
using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Investor.Infrastructure.Converters;
using LooseFunds.Investor.Infrastructure.Entities;
using LooseFunds.Investor.Infrastructure.Repositories;
using LooseFunds.Shared.Toolbox.Core.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Investor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IInvestmentRepository, InvestmentRepository>();
        services.AddSingleton<IDomainObjectConverter<Investment, InvestmentEntity>, InvestmentDomainObjectConverter>();
    }
}