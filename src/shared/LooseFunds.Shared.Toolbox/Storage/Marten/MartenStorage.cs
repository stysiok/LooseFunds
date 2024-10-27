using LooseFunds.Shared.Toolbox.Core.Entity;
using Marten;

namespace LooseFunds.Shared.Toolbox.Storage.Marten;

internal sealed class MartenStorage : IStorage
{
    private readonly IDocumentStore _documentStorage;

    public MartenStorage(IDocumentStore documentStorage)
    {
        _documentStorage = documentStorage;
    }

    public async Task StoreAsync(IEnumerable<DocumentEntity> entities, CancellationToken cancellationToken)
    {
        await using var session = _documentStorage.LightweightSession();

        session.StoreObjects(entities);

        await session.SaveChangesAsync(cancellationToken);
    }
}