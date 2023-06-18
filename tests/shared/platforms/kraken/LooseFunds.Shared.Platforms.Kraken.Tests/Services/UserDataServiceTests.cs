using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Clients;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Services;
using Moq;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Services;

[Category("UnitTests")]
public class UserDataServiceTests
{
    private readonly Fixture _fixture = new();
    private Mock<IKrakenHttpClient> _krakenHttpClient = null!;
    private UserDataService _userDataService = null!;

    [SetUp]
    public void Setup()
    {
        _krakenHttpClient = new();
        _userDataService = new UserDataService(_krakenHttpClient.Object);
    }

    [Test]
    public async Task GetAccountBalanceAsync_returns_account_balance()
    {
        //Arrange
        var cancellationToken = new CancellationToken();

        var balance = _fixture.Create<IReadOnlyDictionary<string, decimal>>();
        _krakenHttpClient
            .Setup(x => x.PostAsync<GetAccountBalance, IReadOnlyDictionary<string, decimal>>(
                It.IsAny<GetAccountBalance>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(balance);

        //Act
        var result = await _userDataService.GetAccountBalanceAsync(cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(balance);

        _krakenHttpClient.Verify(
            x => x.PostAsync<GetAccountBalance, IReadOnlyDictionary<string, decimal>>(It.IsAny<GetAccountBalance>(),
                cancellationToken), Times.Once);
    }
}