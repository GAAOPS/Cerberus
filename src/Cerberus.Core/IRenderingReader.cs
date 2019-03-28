namespace Cerberus.Core
{
    using Data;

    public interface IRenderingReader
    {
        IRendering[] GetRenderings(TreeRoot[] rootPaths);
    }
}