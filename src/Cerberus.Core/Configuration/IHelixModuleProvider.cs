namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;

    public interface IHelixModuleProvider
    {
        IEnumerable<HelixModuleInfo> GetModules();
        HelixModuleInfo GetModuleLayerByPath(string path);
    }
}