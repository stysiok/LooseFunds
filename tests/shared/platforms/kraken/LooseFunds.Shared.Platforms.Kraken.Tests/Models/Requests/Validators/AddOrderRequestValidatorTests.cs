using FluentAssertions;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Validators;

[TestFixture]
[Category("UnitTests")]
public class AddOrderRequestValidatorTests
{
    private readonly AddOrderRequestValidator _validator = new();

    [TestCase(213.32, null)]
    [TestCase(20.321, 12300)]
    public void KrakenRequestValidator_returns_valid_for_valid_request(decimal volume, int? price)
    {
        //Arrange
        var addOrderRequest = new AddOrder(OrderType.limit, Type.buy, volume, Pair.XBTUSD, price);

        //Act
        var result = _validator.Validate(addOrderRequest);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [TestCase(-10.1)]
    [TestCase(null)]
    public void KrakenRequestValidator_throws_exception_when_volume_is_null_or_less_than_zero(decimal volume)
    {
        //Arrange & Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            var _ = new AddOrder(OrderType.limit, Type.buy, volume, Pair.XBTUSD);
        });
    }

    [Test]
    public void KrakenRequestValidator_throws_exception_when_price_is_less_than_zero()
    {
        //Arrange & Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            var _ = new AddOrder(OrderType.limit, Type.buy, 123.32m, Pair.XBTUSD, -1012);
        });
    }
}