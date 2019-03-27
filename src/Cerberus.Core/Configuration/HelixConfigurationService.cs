namespace Cerberus.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Configy;
    using Configy.Containers;
    using Configy.Parsing;
    using Data;

    public class HelixConfigurationService : XmlContainerBuilder
    {
        private readonly IHelixModuleProvider _moduleProvider;
        protected XmlElement ConfigNode;
        protected string ConfigPath;
        protected XmlElement HelixModuleProvider;
        protected IConfiguration[] InternalConfigurations;
        protected XmlElement ValidatorConfig;

        public HelixConfigurationService(string configPath, IHelixModuleProvider moduleProvider,ICerberusVariablesReplacer variablesReplacer) :
            base(
                variablesReplacer)
        {
            ConfigPath = configPath;
            _moduleProvider = moduleProvider;
            
        }

        public IConfiguration[] Configurations
        {
            get
            {
                if (InternalConfigurations == null)
                {
                    LoadConfigurations();
                }

                return InternalConfigurations;
            }
        }

        public virtual Dictionary<string, ITemplate[]> GetTemplates(IConfiguration[] configurations)
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<string, IRendering[]> GetRenderings(IConfiguration[] configurations)
        {
            throw new NotImplementedException();
        }

        protected virtual void LoadConfigurationNodes()
        {
            if (ConfigNode != null)
            {
                return;
            }

            var config = new XmlDocument();
            config.Load(ConfigPath);
            if (config.DocumentElement != null)
            {
                ConfigNode = config.DocumentElement["configurations"];
                ValidatorConfig = config.DocumentElement["validator"];
                ConfigNode = config.DocumentElement["configurations"];
                HelixModuleProvider = config.DocumentElement["init"];
                if (HelixModuleProvider == null)
                {
                    throw new InvalidOperationException("<init> node not found.");
                }

                if (ConfigNode == null)
                {
                    throw new InvalidOperationException("<configurations> node not found.");
                }

                if (ValidatorConfig == null)
                {
                    throw new InvalidOperationException(
                        "<validators> node not found. It should be under <validators> config section.");
                }
            }
            else
            {
                throw new InvalidOperationException("Configuration has not a valid format.");
            }
        }

        protected virtual void LoadVirtualModuleConfigurations()
        {
            var provider = _moduleProvider;
            var helixModules = provider.GetModules();
            foreach (var import in helixModules)
            {
                var xml = new XmlDocument();
                var el = (XmlElement) xml.AppendChild(xml.CreateElement("configuration"));
                el.SetAttribute("name", $"{import.Layer}.{import.Module}");
                el.SetAttribute("extends", "Helix.Base");
                foreach (XmlNode node in ValidatorConfig.ChildNodes)
                {
                    var nodeImport = xml["configuration"]?.OwnerDocument?.ImportNode(node, true);
                    xml["configuration"]?.AppendChild(nodeImport ?? throw new InvalidOperationException(
                                                          $"Can not import the Node: {node.Name}"));
                }

                if (xml.DocumentElement != null)
                {
                    var importedXml = ValidatorConfig.OwnerDocument?.ImportNode(xml.DocumentElement, true);

                    if (importedXml != null)
                    {
                        ConfigNode.AppendChild(importedXml);
                    }
                }
            }
        }

        protected virtual void LoadConfigurations()
        {
            throw new NotImplementedException();
        }

        protected override IContainer CreateContainer(ContainerDefinition definition)
        {
            var description = GetAttributeValue(definition.Definition, "description");

            var attributeValue = GetAttributeValue(definition.Definition, "dependencies");
            var dependencies = !string.IsNullOrEmpty(attributeValue)
                ? attributeValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                : null;

            var ignoredAttributeValue = GetAttributeValue(definition.Definition, "ignoredImplicitDependencies");
            var ignoredDependencies = !string.IsNullOrEmpty(ignoredAttributeValue)
                ? ignoredAttributeValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                : null;

            return new MicroConfiguration(definition.Name, description, definition.Extends, dependencies,
                ignoredDependencies);
        }
    }
}