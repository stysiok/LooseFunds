using LooseFunds.Shared.Platforms.Kraken.Models.Requests.Shared;

namespace LooseFunds.Shared.Platforms.Kraken.Utils;

internal interface IPrivateRequestSigner
{
    string CreateSignature<T>(T request) where T : PrivateKrakenRequest;
}