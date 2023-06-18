using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Shared;

[TestFixture]
[Category("UnitTests")]
public class PrivateKrakenRequestTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public void PrivateKrakenRequest_starts_with_private_prefix()
    {
        //Arrange & Act
        var request = new TestPrivateKrakenRequest("");

        //Assert
        request.Pathname.Should().StartWithEquivalentOf("private/");
    }

    [Test]
    public void PrivateKrakenRequest_correctly_builds_path()
    {
        //Arrange
        var path = _fixture.Create<string>();
        var expected = $"private/{path}";

        //Act
        var request = new TestPrivateKrakenRequest(path);

        //Assert
        request.Pathname.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PrivateKrakenRequest_by_default_has_valid_nonce()
    {
        //Arrange & Act
        var request = new TestPrivateKrakenRequest("");

        //Assert
        request.Nonce.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task PrivateKrakenRequest_nonce_always_has_a_bigger_value()
    {
        //Arrange & Act
        var request1 = new TestPrivateKrakenRequest("");
        await Task.Delay(10);
        var request2 = new TestPrivateKrakenRequest("");
        await Task.Delay(10);
        var request3 = new TestPrivateKrakenRequest("");

        //Assert
        request1.Nonce.Should().BeLessThan(request2.Nonce);
        request1.Nonce.Should().BeLessThan(request3.Nonce);

        request2.Nonce.Should().BeGreaterThan(request1.Nonce);
        request2.Nonce.Should().BeLessThan(request3.Nonce);

        request3.Nonce.Should().BeGreaterThan(request1.Nonce);
        request3.Nonce.Should().BeGreaterThan(request2.Nonce);
    }
}

internal record TestPrivateKrakenRequest : PrivateKrakenRequest
{
    public TestPrivateKrakenRequest(string pathname) : base(pathname)
    {
    }
}