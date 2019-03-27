namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    public class RuleValidationResult : IRuleValidationResult
    {
        public RuleValidationResult(RuleResult result, string message)
        {
            Result = result;
            Message = message;
        }

        public RuleResult Result { get; }
        public string Message { get; }
    }
}