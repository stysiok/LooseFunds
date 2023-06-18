using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Common;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests;

[TestFixture]
[Category("UnitTests")]
public class GetTickerInformationTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public void GetTickerInformation_parameters_have_correct_values()
    {
        //Arrange
        var pairs = new ReadOnlyCollection<Pair>(_fixture.CreateMany<Pair>().ToList());

        //Act
        var request = new GetTickerInformation(pairs);

        //Assert
        request.Pathname.Should().BeEquivalentTo("public/Ticker");
        request.Pairs.Should().BeEquivalentTo(pairs);
    }
}