namespace Sitecore.Helix.Validator.Common.Configuration
{
    public class HelixModuleInfo : IHelixModuleInfo
    {
        public HelixModuleInfo(string path, string root)
        {
            var modularFolders = HelixPathHelper.GetModularFolders(path, root);
            Layer = modularFolders[0];
            Module = modularFolders[1];
            Path = path;
        }

        public HelixModuleInfo(string layer, string module, string path)
        {
            Layer = layer;
            Module = module;
            Path = path;
        }

        public string Layer { get; }
        public string Module { get; }
        public string Path { get; }
        public override string ToString()
        {
            return $"{Layer}.{Module}";
        }
    }
}