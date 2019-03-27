namespace Sitecore.Helix.Validator.Common.Configuration
{
    public interface IHelixModuleInfo
    {
        string Layer { get; }
        string Module { get; }
        string Path { get; }
    }
}