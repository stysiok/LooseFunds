using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace LooseFunds.Investor.Api.E2E.Infrastructure;

internal sealed class TestsContainer : IAsyncDisposable
{
    private readonly PostgreSqlContainer _postgres;
    private readonly RabbitMqContainer _rabbitMq;

    public TestsContainer()
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("LooseFunds")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        _rabbitMq = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.13-management-alpine")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();
    }

    public string PostgresConnectionString => _postgres.GetConnectionString();

    public string RabbitMqHost => $"{_rabbitMq.Hostname}:{_rabbitMq.GetMappedPublicPort(5672)}";

    public string RabbitMqUsername => "guest";

    public string RabbitMqPassword => "guest";

    public async Task StartAsync()
    {
        await _postgres.StartAsync();
        await _rabbitMq.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _rabbitMq.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
