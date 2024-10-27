namespace LooseFunds.Shared.Toolbox.Core.Domain;

public abstract class BusinessRule
{
    public abstract string ErrorMessage { get; }
    public abstract bool Validate();
}

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(BusinessRule rule) : base($"{rule.GetType().Name}: {rule.ErrorMessage}")
    {
    }
}