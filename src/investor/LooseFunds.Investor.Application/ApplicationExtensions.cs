using LooseFunds.Investor.Application.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Investor.Application;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<CreateInvestmentSubscriber>();
    }
}