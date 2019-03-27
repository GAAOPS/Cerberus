namespace Sitecore.Helix.Validator.Common
{
    using System.Collections.Generic;

    public interface ITreeRootFactory
    {
        IEnumerable<TreeRoot> CreateTreeRoots();
    }
}