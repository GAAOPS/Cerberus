namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;
    using Data;

    public interface IHelixLayerInfo : IHelixLayer
    {
        Dictionary<string, IDataElement[]> Modules { get; set; }
    }
}