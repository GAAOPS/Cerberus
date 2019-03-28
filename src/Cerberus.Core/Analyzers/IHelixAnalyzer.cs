namespace Cerberus.Core.Analyzers
{
    public interface IHelixAnalyzer
    {
        string Name { get; }
        IAnalyzeResult Analyze();
    }
}