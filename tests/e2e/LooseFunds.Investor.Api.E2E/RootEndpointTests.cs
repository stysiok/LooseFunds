using System.Net;
using LooseFunds.Investor.Api.E2E.Infrastructure;
using NUnit.Framework;

namespace LooseFunds.Investor.Api.E2E;

[TestFixture]
internal sealed class RootEndpointTests
{
    private TestsContainer? _container;
    private TestApplicationFactory? _factory;
    private HttpClient? _client;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _container = new TestsContainer();
        await _container.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    [SetUp]
    public void SetUp()
    {
        _factory = new TestApplicationFactory(_container!);
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task GetRoot_ReturnsOk()
    {
        using HttpResponseMessage response = await _client!.GetAsync("/");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCase("/verify")]
    [TestCase("/assets")]
    [TestCase("/balance")]
    [TestCase("/ticker")]
    public async Task GetEndpoints_ReturnOk(string path)
    {
        using HttpResponseMessage response = await _client!.GetAsync(path);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetUow_ReturnsOk()
    {
        using HttpResponseMessage response = await _client!.GetAsync("/uow");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
