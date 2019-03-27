namespace Cerberus.Unicorn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Configy.Containers;
    using Configy.Parsing;
    using Core;
    using Core.Configuration;
    using Core.Data;
    using Rainbow.Storage;

    public class RainbowConfigurationService : HelixConfigurationService, IConfigurationProvider
    {
        private readonly IDataSourceLocation _dataSourceLocation;
        private readonly IHelixModuleProvider _helixModuleProvider;

        public RainbowConfigurationService(string configPath, IHelixModuleProvider helixModuleProvider,
            IDataSourceLocation dataSourceLocation,
            ICerberusVariablesReplacer variablesReplacer) :
            base(configPath, helixModuleProvider, variablesReplacer)
        {
            _helixModuleProvider = helixModuleProvider;
            _dataSourceLocation = dataSourceLocation;
        }

        public override Dictionary<string, ITemplate[]> GetTemplates(IConfiguration[] configurations)
        {
            var g = new Dictionary<string, ITemplate[]>();
            foreach (var configuration in configurations)
            {
                var reader = configuration.Resolve<ITemplateReader>();
                var templatePredicate = configuration.Resolve<IPredicate>();
                var root = templatePredicate.GetRootPaths();
                var x = reader.GetTemplates(root);
                g.Add(configuration.Name, x);
            }


            return g;
        }

        public Dictionary<string, ISetting[]> GetSettings(IConfiguration[] configurations)
        {
            var g = new Dictionary<string, ISetting[]>();
            foreach (var configuration in configurations)
            {
                var reader = configuration.Resolve<ISettingsReader>();
                var templatePredicate = configuration.Resolve<IPredicate>();
                var root = templatePredicate.GetRootPaths();
                var x = reader.GetSettings(root);
                g.Add(configuration.Name, x);
            }

            return g;
        }

        public override Dictionary<string, IRendering[]> GetRenderings(IConfiguration[] configurations)
        {
            var g = new Dictionary<string, IRendering[]>();
            foreach (var configuration in configurations)
            {
                var reader = configuration.Resolve<IRenderingReader>();
                var templatePredicate = configuration.Resolve<IPredicate>();
                var root = templatePredicate.GetRootPaths();
                var x = reader.GetRenderings(root);
                g.Add(configuration.Name, x);
            }

            return g;
        }

        protected override void LoadConfigurations()
        {
            LoadConfigurationNodes();

            LoadVirtualModuleConfigurations();

            var configurationNodes = ConfigNode.SelectNodes("./configuration");

            // no configs let's get outta here
            if (configurationNodes == null || configurationNodes.Count == 0)
            {
                InternalConfigurations = new IConfiguration[0];
                return;
            }

            var parser = new XmlContainerParser(ConfigNode, ValidatorConfig, new XmlInheritanceEngine());

            var definitions = parser.GetContainers();

            var configurations = GetContainers(definitions).ToArray();

            foreach (var configuration in configurations)
            {
                configuration.AssertSingleton(typeof(ISourceDataStore));
                configuration.Assert(typeof(IPredicate));
                // register the configuration with itself. how meta!
                configuration.Register(typeof(IConfiguration),
                    () => new ReadOnlyConfiguration((IConfiguration) configuration), true);
                configuration.Register(typeof(IConfigurationProvider), () => this, true);
                configuration.Register(typeof(IHelixModuleProvider), () => _helixModuleProvider, true);
                configuration.Register(typeof(IDataSourceLocation), () => _dataSourceLocation, true);
            }

            InternalConfigurations = configurations
                .Cast<IConfiguration>()
                .Select(config => (IConfiguration) new ReadOnlyConfiguration(config))
                .ToArray();
        }

        protected override void RegisterConfigTypeInterface(IContainer container, Type interfaceType,
            TypeRegistration implementationRegistration, KeyValuePair<string, object>[] unmappedAttributes,
            XmlElement dependency)
        {
            if (interfaceType != typeof(IDataStore))
            {
                base.RegisterConfigTypeInterface(container, interfaceType, implementationRegistration,
                    unmappedAttributes, dependency);
                return;
            }

            // IDataStore registrations get special treatment. The implementation must be disambiguated into Source and Target data stores, 
            // which we do by wrapping it in a ConfigurationDataStore factory and manually registering the apropos interface.
            if ("sourceDataStore".Equals(dependency.Name, StringComparison.OrdinalIgnoreCase))
            {
                container.Register(typeof(ISourceDataStore),
                    () => new ConfigurationDataStore(new Lazy<IDataStore>(() =>
                        (IDataStore) container.Activate(implementationRegistration.Type, unmappedAttributes))),
                    implementationRegistration.SingleInstance);
            }
        }
    }
}