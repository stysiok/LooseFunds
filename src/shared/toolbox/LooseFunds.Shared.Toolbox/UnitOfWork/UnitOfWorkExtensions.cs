using LooseFunds.Shared.Toolbox.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static void AddUnitOfWork(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .Decorate(typeof(INotificationHandler<>), typeof(UnitOfWorkNotificationHandler<>));
}