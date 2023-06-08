using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Models.Exceptions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Clients;

internal sealed class KrakenHttpClient : IKrakenHttpClient
{
    private const string ApiSign = "API-Sign", ApiKey = "API-Key";
    private readonly HttpClient _client;
    private readonly ILogger<KrakenHttpClient> _logger;
    private readonly IPrivateRequestSigner _signer;

    public KrakenHttpClient(HttpClient client, IPrivateRequestSigner signer, IOptions<KrakenOptions> options,
        IOptions<KrakenCredentials> credentials, ILogger<KrakenHttpClient> logger)
    {
        _client = client;
        _signer = signer;
        _logger = logger;

        _client.BaseAddress = new Uri(options.Value.Url!);
        _client.DefaultRequestHeaders.Add(ApiKey, credentials.Value.Key);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken) where TRequest : PrivateKrakenRequest
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Started processing POST {Request}", requestName);

        var signature = _signer.CreateSignature(request);
        _logger.LogDebug("{Request} is private, signature created (signature={Signature})", requestName,
            signature);
        _client.DefaultRequestHeaders.Add(ApiSign, signature);

        var inlinedParams = request.ToInlineParams();
        var content = new StringContent(inlinedParams, Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _client.PostAsync(request.Pathname, content, cancellationToken);
        _logger.LogDebug("{Request} sent and response received (status_code={StatusCode})", requestName,
            response.StatusCode);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var krakenResponse = JsonConvert.DeserializeObject<KrakenResponse<TResponse>>(responseContent);
        _logger.LogDebug("{Request} content deserialized to {Response}", requestName, typeof(TResponse).Name);

        if (krakenResponse != null && krakenResponse.Data != null)
        {
            _logger.LogInformation("Finished processing {Request}, returning {Response}", requestName,
                typeof(TResponse).Name);
            return krakenResponse.Data;
        }

        _logger.LogError("Something went wrong while processing {Request} (errors={Errors})", typeof(TRequest).Name,
            krakenResponse?.Errors);
        throw new InvalidKrakenRequestException(krakenResponse?.Errors);
    }

    public async Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken) where TRequest : PublicKrakenRequest
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Started processing GET {Request}", requestName);

        var inlinedParams = request.ToInlineParams();

        var requestUri = $"{request.Pathname}?{inlinedParams}";

        var response = await _client.GetAsync(requestUri, cancellationToken);
        _logger.LogDebug("{Request} sent and response received (status_code={StatusCode})", requestName,
            response.StatusCode);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var krakenResponse = JsonConvert.DeserializeObject<KrakenResponse<TResponse>>(responseContent);
        _logger.LogDebug("{Request} content deserialized to {Response}", requestName, typeof(TResponse).Name);

        if (krakenResponse != null && krakenResponse.Data != null)
        {
            _logger.LogInformation("Finished processing {Request}, returning {Response}", requestName,
                typeof(TResponse).Name);
            return krakenResponse.Data;
        }

        _logger.LogError("Something went wrong while processing {Request} (errors={Errors})", typeof(TRequest).Name,
            krakenResponse?.Errors);
        throw new InvalidKrakenRequestException(krakenResponse?.Errors);
    }
}