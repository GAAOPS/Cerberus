namespace Cerberus.Core
{
    using System.Collections.Generic;

    public interface ITreeRootFactory
    {
        IEnumerable<TreeRoot> CreateTreeRoots();
    }
}