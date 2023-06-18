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
public class GetAssetInfoTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public void GetAssetInfo_parameters_have_correct_values()
    {
        //Arrange
        var assets = new ReadOnlyCollection<Asset>(_fixture.CreateMany<Asset>().ToList());

        //Act
        var request = new GetAssetInfo(assets);

        //Assert
        request.Pathname.Should().BeEquivalentTo("public/Assets");
        request.Assets.Should().BeEquivalentTo(assets);
    }
}