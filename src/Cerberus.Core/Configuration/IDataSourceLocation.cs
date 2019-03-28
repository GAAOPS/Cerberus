namespace Cerberus.Core.Configuration
{
    public interface IDataSourceLocation
    {
        string DataSourceLocation { get; }
        string SerializationFolder { get; }
        string Root { get; }
    }
}