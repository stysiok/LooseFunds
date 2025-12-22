using LooseFunds.Investor.Api.E2E.Infrastructure.Fakes;
using LooseFunds.Investor.Core.Repositories;
using LooseFunds.Shared.Platforms.Kraken.Services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LooseFunds.Investor.Api.E2E.Infrastructure;

internal sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly TestsContainer _container;

    public TestApplicationFactory(TestsContainer container)
    {
        _container = container;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            Dictionary<string, string?> settings = new()
            {
                ["ConnectionStrings:Marten"] = _container.PostgresConnectionString,
                ["RabbitMQ:Host"] = _container.RabbitMqHost,
                ["RabbitMQ:Username"] = _container.RabbitMqUsername,
                ["RabbitMQ:Password"] = _container.RabbitMqPassword,
                ["kraken:Url"] = "http://localhost/kraken",
                ["kraken:Key"] = "test",
                ["kraken:Secret"] = "test"
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IMarketDataService>();
            services.RemoveAll<IUserDataService>();
            services.RemoveAll<IInvestmentRepository>();
            services.AddScoped<IMarketDataService, FakeMarketDataService>();
            services.AddScoped<IUserDataService, FakeUserDataService>();
            services.AddScoped<IInvestmentRepository, FakeInvestmentRepository>();

            RemoveUnitOfWorkDecorator(services);
        });
    }

    private static void RemoveUnitOfWorkDecorator(IServiceCollection services)
    {
        const string unitOfWorkDecorator =
            "LooseFunds.Shared.Toolbox.MediatR.Decorators.UnitOfWorkNotificationHandlerDecorator`1";

        for (int i = services.Count - 1; i >= 0; i--)
        {
            ServiceDescriptor descriptor = services[i];
            if (descriptor.ServiceType != typeof(INotificationHandler<>)) continue;
            if (descriptor.ImplementationType?.FullName == unitOfWorkDecorator)
            {
                services.RemoveAt(i);
            }
        }
    }
}
