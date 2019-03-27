namespace Cerberus.Core.Analyzers.Rules
{
    public interface IRuleValidationResult
    {
        RuleResult Result { get; }
        string Message { get; }
    }
}