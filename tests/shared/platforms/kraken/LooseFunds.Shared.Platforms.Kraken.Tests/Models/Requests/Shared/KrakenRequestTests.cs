using FluentAssertions;
using FluentValidation;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests.Shared;

[TestFixture]
[Category("UnitTests")]
public class KrakenRequestTests
{
    [TestCase("public/Time")]
    [TestCase("private/Balance")]
    public void KrakenRequest_returns_valid_for_valid_request(string pathname)
    {
        //Arrange & Act
        var request = new TestKrakenRequest(pathname);
        
        //Assert
        request.Should().NotBeNull();
        request.Pathname.Should().BeEquivalentTo(pathname);
    }

    [TestCase("privatadasdas")]
    [TestCase("   ")]
    [TestCase(null)]
    public void KrakenRequest_throws_exception_when_request_is_invalid(string? pathname)
    {
        //Arrange & Act & Assert
        Assert.Throws<ValidationException>(() =>
        {
            var _ = new TestKrakenRequest(pathname);
        });
    }
}

internal record TestKrakenRequest : KrakenRequest
{
    public TestKrakenRequest(string pathname) : base(pathname)
    {
    }
}