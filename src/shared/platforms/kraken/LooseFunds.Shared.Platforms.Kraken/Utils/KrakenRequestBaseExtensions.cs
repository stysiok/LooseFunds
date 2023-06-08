using System.Collections;
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

                var isCollection = p.PropertyType != typeof(string) &&
                                   typeof(IEnumerable).IsAssignableFrom(p.PropertyType);
                string value;
                if (isCollection)
                {
                    var values = (from object? v in p.GetValue(request, null) as IList ?? Array.Empty<object>()
                        select v?.ToString() ?? "").Where(x => !string.IsNullOrEmpty(x)).ToList();
                    value = string.Join(",", values);
                }
                else
                {
                    value = p.GetValue(request)?.ToString() ?? "";
                }

                return string.IsNullOrWhiteSpace(value) ? null : $"{name}={value}";
            })
            .Where(s => !string.IsNullOrEmpty(s)));
        return stringBuilder.ToString();
    }
}