using System.Reflection;
using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Utils;

internal static class KrakenRequestBaseExtensions
{
    internal static string ToInlineParams(this KrakenRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendJoin('&', request.GetType()
            .GetProperties()
            .Where(p => !Attribute.IsDefined(p, typeof(InlineParamsIgnore)))
            .OrderBy(p => p.Name.ToLowerInvariant())
            .Select(p =>
            {
                var jsonPropertyAttribute = p.GetCustomAttribute<JsonPropertyAttribute>();
                var name = jsonPropertyAttribute is null
                    ? p.Name.ToLowerInvariant()
                    : jsonPropertyAttribute.PropertyName;
                var value = p.GetValue(request)?.ToString();
                
                return $"{name}={value}";
            }));
        return stringBuilder.ToString();
    }
}