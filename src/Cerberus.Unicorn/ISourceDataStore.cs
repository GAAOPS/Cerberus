namespace Cerberus.Unicorn
{
    using Rainbow;
    using Rainbow.Storage;

    public interface ISourceDataStore : IDataStore, IDocumentable
    {
        IDataStore InnerDataStore { get; }
    }
}