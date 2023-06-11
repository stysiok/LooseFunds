using System.Linq;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Settings;

[TestFixture]
[Category("UnitTests")]
public class KrakenCredentialsValidatorTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void KrakenCredentialsValidator_returns_valid_for_valid_credentials()
    {
        //Arrange
        var krakenCredentials = _fixture.Create<KrakenCredentials>();
        var validator = new KrakenCredentialsValidator();

        //Act
        var result = validator.Validate(krakenCredentials);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Test]
    public void KrakenCredentialsValidator_validates_if_key_is_empty()
    {
        //Arrange
        var krakenCredentials = _fixture.Build<KrakenCredentials>().Without(kc => kc.Key).Create();
        var validator = new KrakenCredentialsValidator();

        //Act
        var result = validator.Validate(krakenCredentials);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().PropertyName.Should().BeEquivalentTo(nameof(krakenCredentials.Key));
    }
    
    [Test]
    public void KrakenCredentialsValidator_validates_if_secret_is_empty()
    {
        //Arrange
        var krakenCredentials = _fixture.Build<KrakenCredentials>().Without(kc => kc.Secret).Create();
        var validator = new KrakenCredentialsValidator();

        //Act
        var result = validator.Validate(krakenCredentials);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().PropertyName.Should().BeEquivalentTo(nameof(krakenCredentials.Secret));
        
    }
}