namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Collections.Generic;

    public interface IHelixModuleProvider
    {
        IEnumerable<HelixModuleInfo> GetModules();
        HelixModuleInfo GetModuleLayerByPath(string path);
    }
}