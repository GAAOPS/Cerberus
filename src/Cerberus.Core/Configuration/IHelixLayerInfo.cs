namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Collections.Generic;
    using Data;

    public interface IHelixLayerInfo : IHelixLayer
    {
        Dictionary<string, IDataElement[]> Modules { get; set; }
    }
}