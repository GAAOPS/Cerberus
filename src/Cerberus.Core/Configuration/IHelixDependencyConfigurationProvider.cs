namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;

    public interface IHelixDependencyConfigurationProvider
    {
        IList<IHelixLayer> Layers { get; }
    }
}