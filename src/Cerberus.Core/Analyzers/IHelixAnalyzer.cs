namespace Cerberus.Core.Analyzers
{
    public interface IHelixAnalyzer
    {
        IAnalyzeResult Analyze();
        string Name { get; }
    }
}