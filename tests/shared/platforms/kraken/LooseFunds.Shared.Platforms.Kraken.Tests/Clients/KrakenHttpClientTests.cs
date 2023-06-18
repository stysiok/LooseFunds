using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Exceptions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Tests.TestUtils;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Clients;

[Category("UnitTests")]
public class KrakenHttpClientTests
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly IOptions<KrakenCredentials> _credentials;
    private readonly Fixture _fixture = new();
    private readonly IOptions<KrakenOptions> _options;
    private HttpClient _httpClient;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private KrakenHttpClient _krakenHttpClient;
    private string _sign;
    private Mock<IPrivateRequestSigner> _signerMock;

    public KrakenHttpClientTests()
    {
        _options = Options.Create(new KrakenOptions { Url = "https://api.kraken.com/0/" });
        _credentials = Options.Create(_fixture.Create<KrakenCredentials>());
    }

    [SetUp]
    public void Setup()
    {
        _httpMessageHandlerMock = new();
        _sign = _fixture.Create<string>();
        _signerMock = new();
        _signerMock.Setup(x => x.CreateSignature(It.IsAny<PrivateKrakenRequest>()))
            .Returns(_sign);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _krakenHttpClient = new KrakenHttpClient(_httpClient, _signerMock.Object, _options,
            _credentials, NullLogger<KrakenHttpClient>.Instance);
    }

    [Test]
    public void KrakenHttpClient_sets_correct_base_address()
    {
        //Assert
        _httpClient.BaseAddress!.ToString().Should().BeEquivalentTo(_options.Value.Url);
    }

    [Test]
    public void KrakenHttpClient_adds_api_key_header()
    {
        //Assert
        _httpClient.DefaultRequestHeaders.Should().ContainKey("API-Key");
        _httpClient.DefaultRequestHeaders.GetValues("API-Key").Should().AllBeEquivalentTo(_credentials.Value.Key);
    }

    [Test]
    public async Task SendAsync_correctly_processes_request_when_request_is_private()
    {
        //Arrange
        var response = new { result = new { test = "123", value = 321 } };
        var content = JsonConvert.SerializeObject(response);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            });
        var request = new TestPrivateKrakenRequest("Test");

        //Act
        var result = await _krakenHttpClient.SendAsync<TestPrivateKrakenRequest, object>(request, _cancellationToken);

        //Assert
        result.Should().NotBeNull();
        _httpClient.DefaultRequestHeaders.Should().ContainKey("API-Sign");
        _httpClient.DefaultRequestHeaders.GetValues("API-Sign").Should().AllBeEquivalentTo(_sign);
        _signerMock.Verify(
            x => x.CreateSignature(It.Is<PrivateKrakenRequest>(r =>
                r.Pathname == request.Pathname && r.Nonce == request.Nonce)), Times.Once);
        _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task SendAsync_correctly_processes_request_when_request_is_public()
    {
        //Arrange
        var response = new { result = new { test = "123", value = 321 } };
        var content = JsonConvert.SerializeObject(response);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            });
        var request = new TestPublicKrakenRequest("Test");

        //Act
        var result = await _krakenHttpClient.SendAsync<TestPublicKrakenRequest, object>(request, _cancellationToken);

        //Assert
        result.Should().NotBeNull();
        _httpClient.DefaultRequestHeaders.Should().NotContainKey("API-Sign");

        _signerMock.Verify(x => x.CreateSignature(It.IsAny<PrivateKrakenRequest>()), Times.Never);
        _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task SendAsync_throws_http_request_exception_when_http_client_returns_unsuccessful_status_code()
    {
        //Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        //Act
        var act = async () =>
            await _krakenHttpClient.SendAsync<TestPrivateKrakenRequest, object>(new("Test"), _cancellationToken);

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public async Task SendAsync_throws_invalid_kraken_request_when_response_errors_are_not_null()
    {
        //Arrange
        var response = new { error = new[] { "Err!" } };
        var content = JsonConvert.SerializeObject(response);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            });

        //Act
        var act = async () =>
            await _krakenHttpClient.SendAsync<TestPrivateKrakenRequest, object>(new("Test"), _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<InvalidKrakenRequestException>();
    }

    [Test]
    public async Task SendAsync_throws_invalid_kraken_request_when_response_is_null()
    {
        //Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            });

        //Act
        var act = async () =>
            await _krakenHttpClient.SendAsync<TestPrivateKrakenRequest, object>(new("Test"), _cancellationToken);

        //Assert
        await act.Should().ThrowAsync<InvalidKrakenRequestException>();
    }
}