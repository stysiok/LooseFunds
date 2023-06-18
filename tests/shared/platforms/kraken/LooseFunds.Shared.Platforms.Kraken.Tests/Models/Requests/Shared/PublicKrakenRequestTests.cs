using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Shared;

[TestFixture]
[Category("UnitTests")]
public class PublicKrakenRequestTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public void PublicKrakenRequest_starts_with_private_prefix()
    {
        //Arrange & Act
        var request = new TestPublicKrakenRequest("");

        //Assert
        request.Pathname.Should().StartWithEquivalentOf("public/");
    }

    [Test]
    public void PublicKrakenRequest_correctly_builds_path()
    {
        //Arrange
        var path = _fixture.Create<string>();
        var expected = $"public/{path}";

        //Act
        var request = new TestPublicKrakenRequest(path);

        //Assert
        request.Pathname.Should().BeEquivalentTo(expected);
    }
}

internal record TestPublicKrakenRequest : PublicKrakenRequest
{
    public TestPublicKrakenRequest(string pathname) : base(pathname)
    {
    }
}