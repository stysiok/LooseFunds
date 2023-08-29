using Microsoft.AspNetCore.Http;

namespace LooseFunds.Shared.Toolbox.Correlation;

internal sealed class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var hasCorrelationIdHeader = context.Request.Headers.ContainsKey(CorrelationConsts.CorrelationHeader);
        if (!hasCorrelationIdHeader)
            context.Request.Headers.Add(CorrelationConsts.CorrelationHeader, GenerateCorrelationId());

        await _next(context);
    }

    private static string GenerateCorrelationId() => Guid.NewGuid().ToString("N");
}