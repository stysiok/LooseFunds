using LooseFunds.Shared.Toolbox.Core.Domain;

namespace LooseFunds.Investor.Core.Domain.BusinessRules;

internal sealed class TransactionIdIsValid : BusinessRule
{
    private readonly string? _transactionId;
    public override string ErrorMessage => "Transaction id value should not be empty, whitespace, or null, but was";

    public TransactionIdIsValid(string? transactionId)
    {
        _transactionId = transactionId;
    }

    public override bool Validate() => string.IsNullOrWhiteSpace(_transactionId) is false;
}