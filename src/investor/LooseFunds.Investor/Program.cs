using LooseFunds.Shared.Platforms.Kraken;
using LooseFunds.Shared.Platforms.Kraken.Services;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.Run();
