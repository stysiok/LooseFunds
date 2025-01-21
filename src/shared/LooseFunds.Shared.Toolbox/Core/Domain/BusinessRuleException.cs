namespace LooseFunds.Shared.Toolbox.Core.Domain;

public sealed class BusinessRuleException(BusinessRule rule) : Exception($"{rule.GetType().Name}: {rule.ErrorMessage}");
