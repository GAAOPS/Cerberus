namespace Sitecore.Helix.Validator.Common.Analyzers
{
    using System.Collections.Generic;
    using Rules;

    public class AnalyzeResult : IAnalyzeResult
    {
        public AnalyzeResult(List<IRuleValidationResult> results)
        {
            Results = results;
        }

        public List<IRuleValidationResult> Results { get; }
    }
}