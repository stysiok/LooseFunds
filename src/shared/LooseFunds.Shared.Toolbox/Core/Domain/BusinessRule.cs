namespace LooseFunds.Shared.Toolbox.Core.Domain;

public abstract class BusinessRule
{
    public abstract string ErrorMessage { get; }
    public abstract bool Validate();
}