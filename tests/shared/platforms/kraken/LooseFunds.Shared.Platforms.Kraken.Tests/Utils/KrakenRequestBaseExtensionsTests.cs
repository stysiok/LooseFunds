using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Utils;

[TestFixture]
[Category("UnitTests")]
public class KrakenRequestBaseExtensionsTests
{
    [Test]
    public void ToInlineParams_returns_inlined_parameters()
    {
        //Arrange
        var addOrderRequest = new AddOrder(OrderType.limit, Type.buy, 10m, Pair.XBTUSD) { Nonce = 123321 };
        const string expected = "nonce=123321&ordertype=limit&pair=XBTUSD&type=buy&volume=10";

        //Act
        var result = addOrderRequest.ToInlineParams();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }
}