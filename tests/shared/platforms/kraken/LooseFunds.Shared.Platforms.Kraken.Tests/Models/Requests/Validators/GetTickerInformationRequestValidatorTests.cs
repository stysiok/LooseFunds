using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Validators;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Validators;

[Category("UnitTests")]
public class GetTickerInformationRequestValidatorTests
{
    private readonly Fixture _fixture = new();
    private readonly GetTickerInformationRequestValidator _validator = new();

    [Test]
    public void GetTickerInformationRequestValidator_returns_valid_for_valid_request()
    {
        //Arrange
        var getTickerInformation = new GetTickerInformation(_fixture.CreateMany<Pair>().ToList());

        //Act
        var result = _validator.Validate(getTickerInformation);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetTickerInformationRequestValidator_throws_argument_null_exception_when_pairs_are_null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new GetAssetInfo(null);
        });
    }

    [Test]
    public void GetTickerInformationRequestValidator_throws_validation_exception_when_pairs_are_empty()
    {
        Assert.Throws<ValidationException>(() =>
        {
            var _ = new GetAssetInfo(new List<Asset>());
        });
    }
}