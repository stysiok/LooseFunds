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
public class UserTradingServiceTests
{
    private readonly Fixture _fixture = new();
    private Mock<IKrakenHttpClient> _krakenHttpClient = null!;
    private UserTradingService _userTradingService = null!;

    [SetUp]
    public void Setup()
    {
        _krakenHttpClient = new();
        _userTradingService = new UserTradingService(_krakenHttpClient.Object);
    }

    [Test]
    public async Task AddOrderAsync_returns_order_when_valid_params_are_passed()
    {
        //Arrange
        var pair = _fixture.Create<Pair>();
        var volume = _fixture.Create<decimal>();
        var cancellationToken = new CancellationToken();

        var order = _fixture.Create<Order>();
        _krakenHttpClient
            .Setup(x => x.SendAsync<AddOrder, Order>(It.IsAny<AddOrder>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var result = await _userTradingService.AddOrderAsync(pair, volume, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(order);

        _krakenHttpClient.Verify(
            x => x.SendAsync<AddOrder, Order>(It.Is<AddOrder>(o => o.Pair == pair && o.Volume == volume),
                cancellationToken), Times.Once);
    }

    [Test]
    public async Task AddOrderAsync_throws_validation_exception_when_invalid_params_are_passed()
    {
        //Arrange
        var pair = _fixture.Create<Pair>();
        var volume = _fixture.Create<decimal>() * -1;
        var cancellationToken = new CancellationToken();

        //Act
        var act = async () => await _userTradingService.AddOrderAsync(pair, volume, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ValidationException>();
        _krakenHttpClient.Verify(x => x.SendAsync<AddOrder, Order>(It.IsAny<AddOrder>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}