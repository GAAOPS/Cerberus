namespace Cerberus.Core.Configuration
{
    using Analyzers;

    public interface IExitCodePolicy
    {
        int GetExitCodePolicy(IAnalyzeResult result);
    }
}