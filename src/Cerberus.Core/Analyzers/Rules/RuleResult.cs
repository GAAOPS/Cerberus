namespace Cerberus.Core.Analyzers.Rules
{
    using Logging;

    public enum RuleResult
    {
        Success = 0,
        Warning = 1,
        Fail = 2
    }

    public static class RuleResultExtension
    {
        public static LogLevel ToLogLevel(this RuleResult value)
        {
            var result = LogLevel.Information;
            switch (value)
            {
                case RuleResult.Success:
                    result = LogLevel.Information;
                    break;
                case RuleResult.Warning:
                    result = LogLevel.Warning;
                    break;
                case RuleResult.Fail:
                    result = LogLevel.Error;
                    break;
            }

            return result;
        }
    }
}