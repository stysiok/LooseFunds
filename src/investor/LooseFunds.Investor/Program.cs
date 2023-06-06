using LooseFunds.Shared.Platforms.Kraken;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Services;
using LooseFunds.Shared.Toolbox.Correlation;
using LooseFunds.Shared.Toolbox.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKraken(builder.Configuration);
builder.Services.AddCorrelationLogEnricher();

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
        await marketDataService.GetTickerInfoAsync(new List<Pair> { Pair.XBTUSD, Pair.ETHUSD }, cancellationToken));

app.Run();
