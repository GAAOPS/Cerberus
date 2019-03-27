namespace Sitecore.Helix.Validator.Common
{
    using Data;

    public interface IRenderingReader
    {
        IRendering[] GetRenderings(TreeRoot[] rootPaths);
    }
}