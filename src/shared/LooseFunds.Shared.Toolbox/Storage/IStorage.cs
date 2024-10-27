using LooseFunds.Shared.Toolbox.Core.Entity;

namespace LooseFunds.Shared.Toolbox.Storage;

public interface IStorage
{
    Task StoreAsync(IEnumerable<DocumentEntity> entities, CancellationToken cancellationToken);
}