namespace Sitecore.Helix.Validator.Common.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using Configy.Parsing;

    public class ConfigTokenReplacer : ContainerDefinitionVariablesReplacer, IConfigurationTokenReplacer
    {
        private readonly string _configPath;

        public ConfigTokenReplacer(string configPath)
        {
            _configPath =Path.GetFullPath(configPath);
        }

        public override void ReplaceVariables(ContainerDefinition definition)
        {
            ApplyVariables(definition.Definition, new Dictionary<string, string> {{"configPath", _configPath}});
        }
    }
}