namespace Sitecore.Helix.Validator.Common.Analyzers
{
    using System.Collections.Generic;
    using Rules;

    public interface IAnalyzeResult
    {
        List<IRuleValidationResult> Results { get; }
    }
}