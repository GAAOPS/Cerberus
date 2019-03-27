namespace Sitecore.Helix.Validator.Common.Configuration
{
    using Analyzers;

    public interface IExitCodePolicy
    {
        int GetExitCodePolicy(IAnalyzeResult result);
    }
}