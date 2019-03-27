namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    public interface IRuleValidationResult
    {
        RuleResult Result { get; }
        string Message { get; }
    }
}