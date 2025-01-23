using LooseFunds.Investor.Adapters.Kraken;
using LooseFunds.Investor.Application;
using LooseFunds.Investor.Application.Handlers.CreateInvestment;
using LooseFunds.Investor.Core;
using LooseFunds.Investor.Infrastructure;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Services;
using LooseFunds.Shared.Toolbox.Configuration;
using LooseFunds.Shared.Toolbox.Correlation;
using LooseFunds.Shared.Toolbox.Logging;
using LooseFunds.Shared.Toolbox.MediatR;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Storage.Marten;
using LooseFunds.Shared.Toolbox.UnitOfWork;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigurations(builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKrakenAdapter(builder.Configuration, builder.Environment);
builder.Services.AddCorrelationLogEnricher();
builder.Services.AddDomainUnitOfWork();
builder.Services.AddMartenStorage(builder.Configuration, builder.Environment);
builder.Services.AddMediatR(typeof(ApplicationExtensions).Assembly);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddOutbox();
builder.Services.AddCore();

builder.Host.UseLogging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCorrelationId();


app.MapGet("/",
    async (IMarketDataService marketDataService, CancellationToken cancellationToken) =>
        await marketDataService.GetTimeAsync(cancellationToken));

app.MapGet("/verify",
    async (IUserDataService userDataService, CancellationToken cancellationToken) =>
        await userDataService.GetAccountBalanceAsync(cancellationToken));

app.MapGet("/assets",
    async (IMarketDataService marketDataService, CancellationToken cancellationToken) =>
        await marketDataService.GetAssetInfoAsync(new List<Asset> { Asset.XBT, Asset.ETH }, cancellationToken));

app.MapGet("/balance",
    async (IUserDataService userDataService, CancellationToken cancellationToken) =>
        await userDataService.GetAccountBalanceAsync(cancellationToken));

app.MapGet("/ticker",
    async (IMarketDataService marketDataService, CancellationToken cancellationToken) =>
        await marketDataService.GetTickerInfoAsync(Enum.GetValues<Pair>(), cancellationToken));

app.MapGet("/uow",
    async (IMediator mediator, CancellationToken cancellationToken) =>
        await mediator.Publish(new CreateInvestment(), cancellationToken));

app.Run();
