using LooseFunds.Investor.Core.Domain.Events;
using LooseFunds.Shared.Toolbox.Domain;

namespace LooseFunds.Investor.Core.Domain;

public sealed class Investment : DomainObject
{
    public static Investment Create()
        => new Investment(Guid.NewGuid(), null);

    private Investment(Guid id, long? version) : base(id, version)
    {
        AddDomainEvent(new InvestmentCreated(id));
    }
}