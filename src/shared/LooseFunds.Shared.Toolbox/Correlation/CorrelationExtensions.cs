using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.Correlation;

public static class CorrelationExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        => builder.UseMiddleware<CorrelationMiddleware>();

    public static IServiceCollection AddCorrelationLogEnricher(this IServiceCollection services)
        => services.AddHttpContextAccessor()
            .AddSingleton<CorrelationLogEnricher>();
}