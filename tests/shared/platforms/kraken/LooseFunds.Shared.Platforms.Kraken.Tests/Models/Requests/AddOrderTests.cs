using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using NUnit.Framework;
using Type = LooseFunds.Shared.Platforms.Kraken.Models.Requests.Type;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests;

[TestFixture]
[Category("UnitTests")]
public class AddOrderTests
{
    [Test]
    public void AddOrder_parameters_have_correct_values()
    {
        //Arrange
        const OrderType orderType = OrderType.market;
        const Type type = Type.buy;
        const decimal volume = 10.21m;
        const Pair pair = Pair.XBTUSD;
        const int price = 1200;
        
        //Act
        var request = new AddOrder(orderType, type, volume, pair, price);

        //Assert
        request.Pathname.Should().BeEquivalentTo("private/AddOrder");
        request.OrderType.Should().Be(orderType);
        request.Type.Should().Be(type);
        request.Volume.Should().Be(volume);
        request.Pair.Should().Be(pair);
        request.Price.Should().Be(price);
    }
    
    [TestCase(-123.23, Pair.XBTUSD, 100)]
    [TestCase(1233.312, Pair.XBTUSD, -100)]
    public void AddOrder_throws_exception_when_parameter_is_invalid(decimal volume, Pair pair, int price)
    {
        //Act & Assert
        Assert.Throws<FluentValidation.ValidationException>(() =>
        {
            var _ = new AddOrder(OrderType.limit, Type.buy, volume, pair, price);
        });
    }
}