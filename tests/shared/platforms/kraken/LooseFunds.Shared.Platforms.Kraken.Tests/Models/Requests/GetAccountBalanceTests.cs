using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Models.Requests;

[TestFixture]
[Category("UnitTests")]
public class GetAccountBalanceTests
{
    [Test]
    public void GetAccountBalanceTests_parameters_have_correct_values()
    {
        //Act
        var request = new GetAccountBalance();

        //Assert
        request.Pathname.Should().BeEquivalentTo("private/Balance");
    }
}