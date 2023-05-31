using FluentAssertions;
using LooseFunds.Shared.Platforms.Kraken.Models.Requests;
using LooseFunds.Shared.Platforms.Kraken.Settings;
using LooseFunds.Shared.Platforms.Kraken.Utils;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.Utils;

[TestFixture]
[Category("UnitTests")]
public class PrivateRequestSignerTests
{
    private readonly PrivateRequestSigner _privateRequestSigner;

    public PrivateRequestSignerTests()
    {
        var credentials = Options.Create(new KrakenCredentials
        {
            Key = "",
            Secret = "kQH5HW/8p1uGOVjbgWA7FunAmGO8lsSUXNsu3eow76sz84Q18fWxnyRzBHCd3pd5nE9qa99HAZtuZuj6F1huXg=="
        });

        _privateRequestSigner = new PrivateRequestSigner(credentials);
    }

    [Test]
    public void CreateSignature_returns_valid_signature()
    {
        //Arrange
        AddOrder request = new("limit", "buy", 1.25m, "XBTUSD", 37500) { Nonce = 1616492376594 };

        const string expectedSignature =
            "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==";

        //Act
        var signature = _privateRequestSigner.CreateSignature(request);

        //Assert
        signature.Should().BeEquivalentTo(expectedSignature);
    }
}