using LooseFunds.Notifier.Application.Consumers;
using LooseFunds.Shared.Contracts.Investor.Events;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Messaging.Outbox;
using LooseFunds.Shared.Toolbox.Messaging.RabbitMQ;
using LooseFunds.Shared.Toolbox.Storage.Marten;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMessaging(builder.Configuration)
    .AddOutbox()
    .AddMartenStorage(builder.Configuration, builder.Environment)
    .AddConsumer<InvestmentFinishedEvent, InvestmentFinishedConsumer>();

IHost host = builder.Build();

host.Run();
