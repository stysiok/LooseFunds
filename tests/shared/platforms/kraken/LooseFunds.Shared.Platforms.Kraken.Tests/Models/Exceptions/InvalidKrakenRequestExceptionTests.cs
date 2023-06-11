using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Exceptions;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Exceptions;

[TestFixture]
[Category("UnitTests")]
public class InvalidKrakenRequestExceptionTests
{
    [Test]
    public void InvalidKrakenRequestException_has_valid_message()
    {
        //Arrange
        var exception = new InvalidKrakenRequestException(new [] { "123", "321" }); 
        const string expected = "Request returned following errors: 123, 321";
        
        //Assert
        exception.Message.Should().BeEquivalentTo(expected);
    }
}