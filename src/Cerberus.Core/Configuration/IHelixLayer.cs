namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Collections.Generic;

    public interface IHelixLayer
    {
        string Name { get; set; }
        string DependsOn { get; set; }
        IList<IHelixLayer> DependentLayers { get; set; }
    }
}