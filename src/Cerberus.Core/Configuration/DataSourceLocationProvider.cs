namespace Cerberus.Core.Configuration
{
    using System.IO;

    public class DataSourceLocationProvider : IDataSourceLocation
    {
        public DataSourceLocationProvider(string dataSourceLocation, string serializationFolder, string root)
        {
            DataSourceLocation = Path.GetFullPath(dataSourceLocation);
            SerializationFolder = serializationFolder;
            Root = Path.GetFullPath(root);
        }

        public string DataSourceLocation { get; }
        public string SerializationFolder { get; }
        public string Root { get; }
    }
}