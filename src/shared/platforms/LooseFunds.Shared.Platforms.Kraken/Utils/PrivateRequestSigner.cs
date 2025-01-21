using System.Security.Cryptography;
using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LooseFunds.Shared.Platforms.Kraken.Utils;

internal sealed class PrivateRequestSigner : IPrivateRequestSigner
{
    private readonly ILogger<PrivateRequestSigner> _logger;
    private readonly string _secret;

    public PrivateRequestSigner(IOptions<KrakenCredentials> credentials, ILogger<PrivateRequestSigner> logger)
    {
        _logger = logger;
        _secret = credentials.Value.Secret!;
    }

    public string CreateSignature<T>(T request) where T : PrivateKrakenRequest
    {
        string requestName = typeof(T).Name;
        string inlineParams = request.ToInlineParams();
        _logger.LogDebug("{Request} params inlined (inlined_params={InlinedParams})", requestName, inlineParams);

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes($"{request.Nonce.ToString()}{inlineParams}"));
        _logger.LogDebug("{Request} nonce+params hash computed", requestName);

        using var hmac = new HMACSHA512(Convert.FromBase64String(_secret));
        string path = $"/0/{request.Pathname}";
        byte[] hmacDigest = hmac.ComputeHash(Encoding.UTF8.GetBytes(path).Concat(hash).ToArray());
        _logger.LogDebug("{Request} hmac form path and hash computed", requestName);

        return Convert.ToBase64String(hmacDigest);
    }
}
