namespace Cerberus.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class HelixDependencyConfigurationProvider : IHelixDependencyConfigurationProvider
    {
        private List<IHelixLayer> _helixLayers;

        public HelixDependencyConfigurationProvider(XmlNode configNode)
        {
            if (configNode == null)
            {
                return;
            }

            var layers = configNode.ChildNodes.OfType<XmlElement>()
                .Where(node => node.NodeType == XmlNodeType.Element && node.Name.Equals("layer"));

            foreach (var layer in layers)
            {
                Layers.Add(XmlActivator.CreateObject<IHelixLayer>(layer));
            }

            foreach (var helixLayer in Layers)
            {
                var dependencies = helixLayer.DependsOn?.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        var dep = Layers.FirstOrDefault(p =>
                            p.Name.Equals(dependency, StringComparison.InvariantCultureIgnoreCase));
                        helixLayer.DependentLayers.Add(dep as HelixLayer);
                    }
                }
            }
        }

        public IList<IHelixLayer> Layers => _helixLayers ?? (_helixLayers = new List<IHelixLayer>());
    }
}