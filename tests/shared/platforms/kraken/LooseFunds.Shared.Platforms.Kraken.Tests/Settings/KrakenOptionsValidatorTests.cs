using System.Linq;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Settings;

[TestFixture]
[Category("UnitTests")]
public class KrakenOptionsValidatorTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void KrakenCredentialsValidator_returns_valid_for_valid_credentials()
    {
        //Arrange
        var krakenOptions = _fixture.Create<KrakenOptions>();
        var validator = new KrakenOptionsValidator();

        //Act
        var result = validator.Validate(krakenOptions);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Test]
    public void KrakenOptionsValidator_validates_if_key_is_empty()
    {
        //Arrange
        var krakenOptions = _fixture.Build<KrakenOptions>().Without(kc => kc.Url).Create();
        var validator = new KrakenOptionsValidator();

        //Act
        var result = validator.Validate(krakenOptions);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().PropertyName.Should().BeEquivalentTo(nameof(krakenOptions.Url));
    }
}