namespace Cerberus.Rules
{
    using Core.Analyzers.Rules;

    public class TemplateRuleValidationResult : RuleValidationResult, ITemplateRuleValidationResult
    {
        public TemplateRuleValidationResult(RuleResult success, string message = "") : base(success, message)
        {
        }
    }
}