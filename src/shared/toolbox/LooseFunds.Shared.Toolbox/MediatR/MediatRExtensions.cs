using System.Reflection;
using LooseFunds.Shared.Toolbox.MediatR.Decorators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.MediatR;

public static class MediatRExtensions
{
    public static void AddMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });
        services.Decorate(typeof(INotificationHandler<>), typeof(UnitOfWorkNotificationHandlerDecorator<>));
    }
}