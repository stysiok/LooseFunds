using System.Text;
using LooseFunds.Shared.Platforms.Kraken.Models.Exceptions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses.Shared;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LooseFunds.Shared.Platforms.Kraken.Clients;

internal sealed class KrakenHttpClient : IKrakenHttpClient
{
    private const string ApiSign = "API-Sign";
    private readonly IPrivateRequestSigner _signer;
    private readonly HttpClient _client;

    public KrakenHttpClient(HttpClient client, IPrivateRequestSigner signer, IOptions<KrakenOptions> options)
    {
        _client = client;
        _signer = signer;
        
        _client.BaseAddress = new Uri(options.Value.Url!);       
    }

    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, 
        CancellationToken cancellationToken) where TRequest : KrakenRequest where TResponse : IKrakenResponse
    {
        if (request is PrivateKrakenRequest privateRequest)
        {
            var signature = _signer.CreateSignature(privateRequest);
            _client.DefaultRequestHeaders.Add(ApiSign, signature);
        }

        var content = new StringContent(request.ToInlineParams(), Encoding.UTF8,
            "application/x-www-form-urlencoded");

        var httpRequest = new HttpRequestMessage(request.HttpMethod, request.Pathname) { Content = content };
        var response = await _client.SendAsync(httpRequest, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var krakenResponse = JsonConvert.DeserializeObject<KrakenResponse<TResponse>>(responseContent);

        if (krakenResponse != null && krakenResponse.Data != null)
            return krakenResponse.Data;
        
        throw new InvalidKrakenRequestException(krakenResponse?.Errors);
    }
}