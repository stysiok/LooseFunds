using System.Linq;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Validators;

[TestFixture]
[Category("UnitTests")]
public class PrivateKrakenRequestValidatorTests
{
    private readonly PrivateKrakenRequestValidator _validator = new();
    
    [Test]
    public void PrivateKrakenRequestValidator_returns_valid_for_valid_private_request()
    {
        //Arrange
        var privateKrakenRequest = new AddOrder(OrderType.limit, Type.buy, 10m, Pair.XBTUSD) { Nonce = 123321 };

        //Act
        var result = _validator.Validate(privateKrakenRequest);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [TestCase(0)]
    [TestCase(-500100900)]
    public void PrivateKrakenRequestValidator_validates_if_nonce_is_greater_than_zero(long nonce)
    {
        //Arrange
        var privateKrakenRequest = new AddOrder(OrderType.limit, Type.buy, 10m, Pair.XBTUSD) { Nonce = nonce };

        //Act
        var result = _validator.Validate(privateKrakenRequest);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.First().PropertyName.Should().BeEquivalentTo(nameof(PrivateKrakenRequest.Nonce));
    }
}