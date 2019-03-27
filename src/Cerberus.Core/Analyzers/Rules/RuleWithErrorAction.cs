namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    public class RuleWithErrorAction : IRule
    {
        private readonly ErrorAction _errorAction;

        public RuleWithErrorAction(ErrorAction errorAction)
        {
            _errorAction = errorAction;
        }

        protected virtual RuleResult GetFailResult()
        {
            return _errorAction == ErrorAction.Warning ? RuleResult.Warning : RuleResult.Fail;
        }
    }
}