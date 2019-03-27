namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    public class RenderingRuleValidationResult : RuleValidationResult, IRenderingRuleValidationResult
    {
        public RenderingRuleValidationResult(RuleResult success, string message = "") : base(success, message)
        {
        }
    }
}