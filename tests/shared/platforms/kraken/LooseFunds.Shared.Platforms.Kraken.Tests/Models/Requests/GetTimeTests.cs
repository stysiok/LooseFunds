using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests;

[TestFixture]
[Category("UnitTests")]
public class GetTimeTests
{
    [Test]
    public void GetTime_parameters_have_correct_values()
    {
        //Act
        var request = new GetTime();

        //Assert
        request.Pathname.Should().BeEquivalentTo("public/Time");
    }
}