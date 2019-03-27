namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Xml;
    using Analyzers;
    using Configy;
    using Configy.Parsing;

    public sealed class ConfigInitializer : XmlContainerBuilder
    {
        public ConfigInitializer(string configFilePath) : this(new ConfigTokenReplacer(configFilePath))
        {
            var config = new XmlDocument();
            config.Load(configFilePath);
            var initConfig = config.DocumentElement?["init"];
            var provider = GetContainer(new ContainerDefinition(initConfig));
            ConfigurationProvider = provider.Resolve<IConfigurationProvider>();
            HelixDependencyConfigurationProvider = provider.Resolve<IHelixDependencyConfigurationProvider>();
            HelixAnalyzerService = provider.Resolve<IHelixAnalyzerService>();
            ExitCodePolicyService = provider.Resolve<IExitCodePolicy>();
        }

        public ConfigInitializer(IContainerDefinitionVariablesReplacer variablesReplacer) : base(variablesReplacer)
        {
        }

        public IExitCodePolicy ExitCodePolicyService { get; set; }

        public IHelixAnalyzerService HelixAnalyzerService { get; set; }

        public IHelixDependencyConfigurationProvider HelixDependencyConfigurationProvider { get; }

        public IConfigurationProvider ConfigurationProvider { get; }
    }
}