using LooseFunds.Shared.Toolbox.Core.Domain;
using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.UnitOfWork;

internal interface ITrackable
{
    DomainObject Tracked { get; }
    DocumentEntity Convert();
}
