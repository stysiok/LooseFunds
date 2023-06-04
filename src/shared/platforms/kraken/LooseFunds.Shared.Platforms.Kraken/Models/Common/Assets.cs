using System.Collections.ObjectModel;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Common;

public sealed class Assets : ReadOnlyCollection<Asset>
{
    public Assets(IList<Asset> list) : base(list)
    {
    }
    
    //Items property comes from ReadOnlyCollection implementation
    public override string ToString() => string.Join(',', Items);
}