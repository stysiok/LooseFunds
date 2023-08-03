using MediatR;
using Microsoft.Extensions.Logging;

namespace LooseFunds.Shared.Toolbox.MediatR.Decorators;

internal sealed class LoggingNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
{
    private readonly INotificationHandler<T> _handler;
    private readonly ILogger<LoggingNotificationHandlerDecorator<T>> _logger;

    public LoggingNotificationHandlerDecorator(INotificationHandler<T> handler,
        ILogger<LoggingNotificationHandlerDecorator<T>> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        var name = typeof(T).Name;
        _logger.LogInformation("Started processing notification... [name={name}]", name);
        try
        {
            await _handler.Handle(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception thrown while processing notification... [name={name}, exception={exception}, message={message}]",
                name, e.GetType().Name, e.Message);
            throw;
        }

        _logger.LogInformation("Finished processing notification... [name={name}]", name);
    }
}
