using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain;

public sealed class Investment : DomainObject
{
    private Investment(Guid id, long? version) : base(id, version)
    {
        AddDomainEvent(new InvestmentCreated(id));
    }

    public static Investment Create()
        => new(Guid.NewGuid(), null);
}