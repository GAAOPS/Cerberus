namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class HelixModuleProvider : IHelixModuleProvider
    {
        private readonly IDataSourceLocation _dataSourceLocation;
        private readonly string _pattern;
        private List<HelixModuleInfo> _modules;

        public HelixModuleProvider(IDataSourceLocation dataSourceLocation,string pattern)
        {
            _dataSourceLocation = dataSourceLocation;
            _pattern = pattern;
        }

        public IEnumerable<HelixModuleInfo> GetModules()
        {
            var root = Path.GetFullPath(_dataSourceLocation.DataSourceLocation);
            var serializationFolder = _dataSourceLocation.SerializationFolder;
            if (!Directory.Exists(root))
            {
                throw new InvalidOperationException($"<DataSourceLocation> is not valid: {root}");
            }

            return _modules ?? (_modules = Directory
                       .GetDirectories(root, serializationFolder, SearchOption.AllDirectories)
                       .Where(p => HelixPathHelper.IsSerializationFolder(p, root, serializationFolder))
                       .Select(p => new HelixModuleInfo(p, root)).ToList());
        }

        public HelixModuleInfo GetModuleLayerByPath(string path)
        {
            string layer = "";
            string module = "";
            var patternPath = _pattern.ToLower().Replace("$configpath", String.Empty).Split(new[] { '\\' }).ToList();
            var relativePath = path.Replace(_dataSourceLocation.DataSourceLocation, String.Empty).Split(new[] {'\\'}).ToList();
            var indexOfLayer = patternPath.IndexOf("$layer");
            var indexOfModule = patternPath.IndexOf("$module");
            if (relativePath.Count > indexOfLayer)
            {
                layer = relativePath[indexOfLayer];
            }

            if (relativePath.Count > indexOfModule)
            {
                module = relativePath[indexOfModule];
            }

            return _modules.FirstOrDefault(p =>
                p.Layer.Equals(layer, StringComparison.CurrentCultureIgnoreCase) &&
                p.Module.Equals(module, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}