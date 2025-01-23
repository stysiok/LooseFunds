using LooseFunds.Processor.Application.Workers;
using LooseFunds.Shared.Toolbox.Logging;
using LooseFunds.Shared.Toolbox.Messaging;
using LooseFunds.Shared.Toolbox.Storage.Marten;
using LooseFunds.Shared.Toolbox.UnitOfWork;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddLogging(builder.Configuration, builder.Environment)
    .AddMessaging(builder.Configuration)
    .AddUnitOfWork()
    .AddOutbox()
    .AddMartenStorage(builder.Configuration, builder.Environment)
    .AddHostedService<OutboxWorker>();

IHost host = builder.Build();

host.Run();
