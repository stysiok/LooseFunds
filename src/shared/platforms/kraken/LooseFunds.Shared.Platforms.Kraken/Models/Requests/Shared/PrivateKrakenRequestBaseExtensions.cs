using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Utils;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal static class KrakenRequestBaseExtensions
{
    internal static string ToInlineParams(this KrakenRequest request)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendJoin('&', request.GetType()
            .GetProperties()
            .Where(p => !Attribute.IsDefined(p, typeof(InlineParamsIgnore)))
            .OrderBy(p => p.Name.ToLowerInvariant())
            .Select(p => $"{p.Name.ToLowerInvariant()}={p.GetValue(request)}"));
        return stringBuilder.ToString();
    }
}