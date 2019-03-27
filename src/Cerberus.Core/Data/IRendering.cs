namespace Sitecore.Helix.Validator.Common.Data
{
    public interface IRendering : IDataElement
    {
        RenderingField[] Fields { get; set; }
    }
}