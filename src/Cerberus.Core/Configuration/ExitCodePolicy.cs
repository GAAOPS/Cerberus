namespace Cerberus.Core.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Analyzers;
    using Analyzers.Rules;

    public class ExitCodePolicy : IExitCodePolicy
    {
        private RuleResult _failOn;

        public ExitCodePolicy(string failOn)
        {
            if (!Enum.TryParse(failOn, true, out _failOn))
            {
                throw new InvalidEnumArgumentException("Fail condition on export policy is not correct.");
            }
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
            if (_failOn == RuleResult.None)
            {
                return 0;
            }

            if (_failOn == RuleResult.Warning && result == RuleResult.Warning)
            {
                return 1;
            }

            if (_failOn == RuleResult.Fail && result != RuleResult.Fail)
            {
                return 0;
            }
            // everything else is Fail
            return 2;
        }
    }
}