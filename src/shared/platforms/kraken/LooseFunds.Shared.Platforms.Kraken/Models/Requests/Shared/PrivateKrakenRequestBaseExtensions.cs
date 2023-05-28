using System.Text;

namespace LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

internal static class KrakenRequestBaseExtensions
{
    internal static string ToInlineParams(this KrakenRequest request)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendJoin('&', request.GetType().GetProperties().Select(p => 
            $"{p.Name.ToLowerInvariant()}={p.GetValue(request)}"));
        return stringBuilder.ToString();
    }
}