using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace LooseFunds.Shared.Toolbox.Correlation;

internal sealed class CorrelationLogEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationLogEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null ||
            !httpContext.Request.Headers.TryGetValue(CorrelationConsts.CorrelationHeader, out var values) ||
            !values.Any()) return;
        
        var correlationId = values.First();
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(CorrelationConsts.CorrelationLogName,
            correlationId));
    }
}

