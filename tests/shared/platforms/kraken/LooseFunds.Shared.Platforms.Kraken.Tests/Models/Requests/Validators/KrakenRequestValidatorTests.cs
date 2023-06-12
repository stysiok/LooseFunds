using FluentAssertions;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Validators;

[TestFixture]
[Category("UnitTests")]
public class KrakenRequestValidatorTests
{
    private readonly KrakenRequestValidator _validator = new();
    
    [TestCase("private/")]
    [TestCase("public/")]
    public void KrakenRequestValidator_returns_valid_for_valid_request(string validPrefix)
    {
        //Arrange
        var krakenRequest = new TestKrakenRequest(validPrefix);

        //Act
        var result = _validator.Validate(krakenRequest);
        
        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [TestCase("123123123123")]
    [TestCase("random_text_aha")]
    [TestCase(null)]
    [TestCase("   public")]
    [TestCase("privpublic")]
    public void KrakenRequestValidator_throws_exception_when_path_starts_with_incorrect_prefix(string invalidPrefix)
    {
        //Arrange & Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            var _ = new TestKrakenRequest(invalidPrefix);
        });
    }
}

internal record TestKrakenRequest : KrakenRequest
{
    public TestKrakenRequest(string pathname) : base(pathname)
    {
    }
}