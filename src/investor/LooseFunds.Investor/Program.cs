using LooseFunds.Shared.Platforms.Kraken;
using LooseFunds.Shared.Platforms.Kraken.Services;
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


app.MapGet("/", async (IMarketDataService marketDataService, CancellationToken cancellationToken) =>
{
    var result = await marketDataService.GetTimeAsync(cancellationToken);
    return result.Date;
});

app.MapGet("/verify", async (IUserDataService userDataService, CancellationToken cancellationToken) =>
{
    var result = await userDataService.GetAccountBalanceAsync(cancellationToken);
    return result;
});

app.Run();
