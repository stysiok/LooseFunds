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
    private const string ApiSign = "API-Sign";
    private readonly IPrivateRequestSigner _signer;
    private readonly ILogger<KrakenHttpClient> _logger;
    private readonly HttpClient _client;

    public KrakenHttpClient(HttpClient client, IPrivateRequestSigner signer, IOptions<KrakenOptions> options,
        ILogger<KrakenHttpClient> logger)
    {
        _client = client;
        _signer = signer;
        _logger = logger;

        _client.BaseAddress = new Uri(options.Value.Url!);       
    }

    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, 
        CancellationToken cancellationToken) where TRequest : KrakenRequest
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Started processing {Request}", requestName);
        if (request is PrivateKrakenRequest privateRequest)
        {
            var signature = _signer.CreateSignature(privateRequest);
            _logger.LogDebug("{Request} is private, signature created (signature={Signature})", requestName,
                signature);
            _client.DefaultRequestHeaders.Add(ApiSign, signature);
        }

        var inlinedParams = request.ToInlineParams();
        var content = new StringContent(inlinedParams, Encoding.UTF8,
            "application/x-www-form-urlencoded");

        var httpRequest = new HttpRequestMessage(request.HttpMethod, request.Pathname) { Content = content };
        var response = await _client.SendAsync(httpRequest, cancellationToken);
        _logger.LogDebug("{Request} sent and response received (status_code={StatusCode})", requestName,
            response.StatusCode);
        
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var krakenResponse = JsonConvert.DeserializeObject<KrakenResponse<TResponse>>(responseContent);
        _logger.LogDebug("{Request} content deserialized to {Response}", requestName, typeof(TResponse).Name);

        if (krakenResponse != null && krakenResponse.Data != null)
        {
            _logger.LogInformation("Finished processing {Request}, returing {Response}", requestName, typeof(TResponse).Name);
            return krakenResponse.Data;
        }
        
        _logger.LogError("Something went wrong while processing {Request} (errors={Errors})", typeof(TRequest).Name,
            krakenResponse?.Errors);
        throw new InvalidKrakenRequestException(krakenResponse?.Errors);
    }
}