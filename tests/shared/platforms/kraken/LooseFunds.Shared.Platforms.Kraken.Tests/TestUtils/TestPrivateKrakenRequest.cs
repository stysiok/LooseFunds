using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Tests.TestUtils;

internal record TestPrivateKrakenRequest : PrivateKrakenRequest
{
    public TestPrivateKrakenRequest(string pathname) : base(pathname)
    {
    }
}