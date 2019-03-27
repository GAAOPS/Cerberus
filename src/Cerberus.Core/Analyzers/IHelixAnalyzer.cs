namespace Sitecore.Helix.Validator.Common.Analyzers
{
    public interface IHelixAnalyzer
    {
        IAnalyzeResult Analyze();
        string Name { get; }
    }
}