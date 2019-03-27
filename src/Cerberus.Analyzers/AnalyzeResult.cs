namespace Cerberus.Analyzers
{
    using System.Collections.Generic;
    using Core.Analyzers;
    using Core.Analyzers.Rules;

    public class AnalyzeResult : IAnalyzeResult
    {
        public AnalyzeResult(List<IRuleValidationResult> results)
        {
            Results = results;
        }

        public List<IRuleValidationResult> Results { get; }
    }
}