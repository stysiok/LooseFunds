using LooseFunds.Notifier.Application.Subscribers;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Storage.Marten;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMessaging(builder.Configuration)
    .AddOutbox()
    .AddMartenStorage(builder.Configuration, builder.Environment)
    .AddHostedService<InvestmentCreatedEventSubscriber>();

IHost host = builder.Build();

host.Run();
