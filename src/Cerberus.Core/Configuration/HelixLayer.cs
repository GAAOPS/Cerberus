namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;

    public class HelixLayer : IHelixLayer
    {
        public HelixLayer()
        {
            DependentLayers = new List<IHelixLayer>();
        }

        public string Name { get; set; }
        public string DependsOn { get; set; }
        public IList<IHelixLayer> DependentLayers { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Depends on : {DependsOn}";
        }
    }
}