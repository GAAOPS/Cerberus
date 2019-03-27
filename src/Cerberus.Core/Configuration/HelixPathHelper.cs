namespace Cerberus.Core.Configuration
{
    using System;
    using System.IO;
    using System.Linq;

    internal static class HelixPathHelper
    {
        internal static string[] GetModularFolders(string path, string root)
        {
            var relative = path.Replace(root, string.Empty).Split(Path.DirectorySeparatorChar).Where(p => p.Length > 0);
            return relative.ToArray();
        }

        internal static bool IsSerializationFolder(string path, string root, string serializationFolder)
        {
            var relative = GetModularFolders(path, root);
            return relative.Length == 3 && relative[2].Equals(serializationFolder,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}