namespace Cerberus.Core
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [DebuggerDisplay("{Name} {DatabaseName}:{Path}")]
    public class TreeRoot
    {
        public TreeRoot(string name, string path, string databaseName)
        {
            Name = name;
            Path = path;
            DatabaseName = databaseName;
        }

        public string Name { get; protected set; }

        public string Path { get; protected set; }

        public string DatabaseName { get; protected set; }
    }
}