namespace Cerberus.Rules
{
    using Core.Analyzers.Rules;

    public class RenderingRuleValidationResult : RuleValidationResult, IRenderingRuleValidationResult
    {
        public RenderingRuleValidationResult(RuleResult success, string message = "") : base(success, message)
        {
        }
    }
}