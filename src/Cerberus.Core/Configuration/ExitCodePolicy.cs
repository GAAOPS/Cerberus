namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System;
    using System.Linq;
    using Analyzers;
    using Analyzers.Rules;

    public class ExitCodePolicy : IExitCodePolicy
    {
        private readonly string _failOn;

        public ExitCodePolicy(string failOn)
        {
            _failOn = failOn;
        }

        public int GetExitCodePolicy(IAnalyzeResult result)
        {
            if (result.Results.Any(p => p.Result == RuleResult.Fail))
            {
                return GetPolicyResult(RuleResult.Fail);
            }

            return result.Results.Any(p => p.Result == RuleResult.Warning)
                ? GetPolicyResult(RuleResult.Warning)
                : 0;
        }

        private int GetPolicyResult(RuleResult result)
        {
            if (_failOn.Equals("None", StringComparison.InvariantCultureIgnoreCase))
            {
                return 0;
            }

            if (_failOn.Equals("Warning", StringComparison.InvariantCultureIgnoreCase) && result == RuleResult.Warning)
            {
                return 1;
            }

            // everything else is Fail
            return 2;
        }
    }
}