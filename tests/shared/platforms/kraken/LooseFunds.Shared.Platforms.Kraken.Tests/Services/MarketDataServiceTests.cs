using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Responses;
using LooseFunds.Shared.Platforms.Kraken.Services;
using Moq;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Services;

[Category("UnitTests")]
public class MarketDataServiceTests
{
    private readonly Fixture _fixture = new();
    private Mock<IKrakenHttpClient> _krakenHttpClient = null!;
    private MarketDataService _marketDataService = null!;

    [SetUp]
    public void Setup()
    {
        _krakenHttpClient = new();
        _marketDataService = new MarketDataService(_krakenHttpClient.Object);
    }

    [Test]
    public async Task GetTimeAsync_returns_time()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var time = _fixture.Create<Time>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetTime, Time>(It.IsAny<GetTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(time);

        //Act
        var result = await _marketDataService.GetTimeAsync(cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(time);

        _krakenHttpClient.Verify(x => x.SendAsync<GetTime, Time>(It.IsAny<GetTime>(), cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetAssetInfoAsync_returns_assets()
    {
        //Arrange
        var assets = _fixture.CreateMany<Asset>().ToList();
        var cancellationToken = new CancellationToken();

        var assetInfos = _fixture.Create<IReadOnlyDictionary<string, AssetInfo>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(It.IsAny<GetAssetInfo>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(assetInfos);

        //Act
        var result = await _marketDataService.GetAssetInfoAsync(assets, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(assetInfos);

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(
                It.Is<GetAssetInfo>(ai => ai.Assets.All(assets.Contains)), cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetAssetInfoAsync_throws_argument_exception_when_assets_are_null()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var assetInfos = _fixture.Create<IReadOnlyDictionary<string, AssetInfo>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(It.IsAny<GetAssetInfo>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(assetInfos);

        //Act
        var act = async () => await _marketDataService.GetAssetInfoAsync(null, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ArgumentException>();

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(It.IsAny<GetAssetInfo>(),
                cancellationToken), Times.Never);
    }

    [Test]
    public async Task GetAssetInfoAsync_throws_validation_exception_when_assets_are_empty()
    {
        //Arrange
        var emptyAssets = new List<Asset>();
        var cancellationToken = new CancellationToken();

        var assetInfos = _fixture.Create<IReadOnlyDictionary<string, AssetInfo>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(It.IsAny<GetAssetInfo>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(assetInfos);

        //Act
        var act = async () => await _marketDataService.GetAssetInfoAsync(emptyAssets, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ValidationException>();

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetAssetInfo, IReadOnlyDictionary<string, AssetInfo>>(It.IsAny<GetAssetInfo>(),
                cancellationToken), Times.Never);
    }

    [Test]
    public async Task GetTickerInfoAsync_returns_tickers()
    {
        //Arrange
        var pairs = _fixture.CreateMany<Pair>().ToList();
        var cancellationToken = new CancellationToken();

        var tickers = _fixture.Create<IReadOnlyDictionary<string, Ticker>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.IsAny<GetTickerInformation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickers);

        //Act
        var result = await _marketDataService.GetTickerInfoAsync(pairs, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(tickers);

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.Is<GetTickerInformation>(ti => ti.Pairs.All(pairs.Contains)), cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetTickerInfoAsync_throws_validation_exception_when_pairs_are_empty()
    {
        //Arrange
        var emptyPairs = new List<Pair>();
        var cancellationToken = new CancellationToken();

        var tickers = _fixture.Create<IReadOnlyDictionary<string, Ticker>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.IsAny<GetTickerInformation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickers);

        //Act
        var act = async () => await _marketDataService.GetTickerInfoAsync(emptyPairs, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ValidationException>();

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.IsAny<GetTickerInformation>(),
                cancellationToken), Times.Never);
    }

    [Test]
    public async Task GetTickerInfoAsync_throws_argument_exception_when_pairs_are_null()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var tickers = _fixture.Create<IReadOnlyDictionary<string, Ticker>>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.IsAny<GetTickerInformation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickers);

        //Act
        var act = async () => await _marketDataService.GetTickerInfoAsync(null, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ArgumentException>();

        _krakenHttpClient.Verify(
            x => x.SendAsync<GetTickerInformation, IReadOnlyDictionary<string, Ticker>>(
                It.IsAny<GetTickerInformation>(),
                cancellationToken), Times.Never);
    }
}