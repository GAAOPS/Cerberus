namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;
    using Data;

    public class HelixLayerInfo : IHelixLayerInfo
    {
        public HelixLayerInfo(IHelixLayer layer)
        {
            Name = layer.Name;
            DependsOn = layer.DependsOn;
            DependentLayers = layer.DependentLayers;
            Modules = new Dictionary<string, IDataElement[]>();
        }

        public string Name { get; set; }
        public string DependsOn { get; set; }
        public IList<IHelixLayer> DependentLayers { get; set; }
        public Dictionary<string, IDataElement[]> Modules { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}