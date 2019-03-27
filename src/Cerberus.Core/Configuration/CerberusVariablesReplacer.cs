namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Configy.Containers;
    using Configy.Parsing;

    public class CerberusVariablesReplacer : ContainerDefinitionVariablesReplacer, ICerberusVariablesReplacer
    {
        private readonly IContainerDefinitionVariablesReplacer[] _replacerCollection;
        public CerberusVariablesReplacer(XmlNode configNode)
        {
            var replacerNodes = configNode.ChildNodes.OfType<XmlElement>()
                .Where(nd => nd.NodeType == XmlNodeType.Element && nd.Name.Equals("replacer"));

            var ruleInstances = new List<IContainerDefinitionVariablesReplacer>();

            foreach (var node in replacerNodes)
            {
                var type = XmlActivator.GetType(node);

                ruleInstances.Add(XmlActivator.CreateInstance(type, node, new IContainer[] { }) as IContainerDefinitionVariablesReplacer);
            }

            _replacerCollection = ruleInstances.ToArray();

        }


        public override void ReplaceVariables(ContainerDefinition definition)
        {
            foreach (var instance in _replacerCollection)
            {
                instance.ReplaceVariables(definition);
            }
        }
    }

    public interface ICerberusVariablesReplacer : IContainerDefinitionVariablesReplacer
    {
    }
}