using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.TestUtils;

internal record TestPublicKrakenRequest : PublicKrakenRequest
{
    public TestPublicKrakenRequest(string pathname) : base(pathname)
    {
    }
}