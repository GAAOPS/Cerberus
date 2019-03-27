namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Collections.Generic;

    public interface IHelixDependencyConfigurationProvider
    {
        IList<IHelixLayer> Layers { get; }
    }
}