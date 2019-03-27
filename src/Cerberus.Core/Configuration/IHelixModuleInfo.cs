namespace Cerberus.Core.Configuration
{
    public interface IHelixModuleInfo
    {
        string Layer { get; }
        string Module { get; }
        string Path { get; }
    }
}