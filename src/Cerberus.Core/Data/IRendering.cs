namespace Cerberus.Core.Data
{
    public interface IRendering : IDataElement
    {
        RenderingField[] Fields { get; set; }
    }
}