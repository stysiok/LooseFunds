using Microsoft.AspNetCore.Http;

namespace LooseFunds.Shared.Toolbox.Correlation;

internal sealed class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var hasCorrelationIdHeader = context.Request.Headers.ContainsKey(CorrelationConsts.CorrelationHeader);
        if (hasCorrelationIdHeader is false)
            context.Request.Headers.Add(CorrelationConsts.CorrelationHeader, Guid.NewGuid().ToString("N"));

        await _next(context);
    }
}

internal interface ICorrelationAccessor
{
    string CorrelationId { get; }
}

internal interface ICorrelationSetter
{
    void Set(Guid correlationId);
}

internal sealed class CorrelationContainer : ICorrelationAccessor, ICorrelationSetter
{
    private string? _correlationId;

    public string CorrelationId => _correlationId ?? throw new Exception();

    public void Set(Guid correlationId)
    {
        _correlationId = correlationId.ToString("N");
    }
}