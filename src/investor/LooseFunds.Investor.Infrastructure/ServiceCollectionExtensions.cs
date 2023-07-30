using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Investor.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Investor.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInvestmentRepository(this ServiceCollection services)
    {
        services.AddScoped<IInvestmentRepository, InvestmentRepository>();
    }
}