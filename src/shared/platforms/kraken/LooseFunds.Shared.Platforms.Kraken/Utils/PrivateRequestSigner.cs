using System.Security.Cryptography;
using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using Microsoft.Extensions.Options;

namespace LooseFunds.Shared.Platforms.Kraken.Utils;

internal sealed class PrivateRequestSigner : IPrivateRequestSigner
{
    private readonly KrakenCredentials _credentials;
    private readonly KrakenOptions _options;

    public PrivateRequestSigner(IOptions<KrakenCredentials> credentials, IOptions<KrakenOptions> options)
    {
        _credentials = credentials.Value; 
        _options = options.Value;
    }

    public string CreateSignature<T>(T request) where T : PrivateKrakenRequest
    {
        using var sha256 = SHA256.Create();
        var inlineParams = request.ToInlineParams();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{request.Nonce.ToString()}{inlineParams}"));
            
        using var hmac = new HMACSHA512(Convert.FromBase64String(_credentials.Secret!));
        var path = $"{_options.Url}{request.Pathname}";
        var hmacDigest = hmac.ComputeHash(Encoding.UTF8.GetBytes(path).Concat(hash).ToArray());

        return Convert.ToBase64String(hmacDigest);
    }
}