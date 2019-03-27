namespace Cerberus.Analyzers
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Analyzers;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;
    using Rules;

    public class SettingsAnalyzer : Analyzer<ISetting>, ISettingsAnalyzer
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IHelixDependencyConfigurationProvider _dependencyConfigurationProvider;
        private readonly IEnumerable<ISettingRule> _rules;

        public SettingsAnalyzer(IConfigurationProvider configurationProvider,
            IHelixDependencyConfigurationProvider dependencyConfigurationProvider,
            IEnumerable<IRule> rules)
        {
            _configurationProvider = configurationProvider;
            _dependencyConfigurationProvider = dependencyConfigurationProvider;
            _rules = rules.Cast<ISettingRule>();
            Name = "Settings";
        }

        public IAnalyzeResult Analyze()
        {
            var configs = _configurationProvider.Configurations;
            var settings = _configurationProvider.GetSettings(configs).Where(p => p.Value.Length > 0)
                .ToDictionary(d => d.Key, d => d.Value);
            var allLayers = GetHelixModules(settings, _dependencyConfigurationProvider.Layers).ToList();
            var allTemplates = GetAllSettings(settings).ToList();
            var validationResult = new List<IRuleValidationResult>();

            foreach (var layer in allLayers)
            {
                validationResult.AddRange(ProcessLayer(allLayers, layer, allTemplates));
            }

            return new AnalyzeResult(validationResult);
        }

        private List<IRuleValidationResult> ProcessLayer(List<IHelixLayerInfo> allLayers, IHelixLayerInfo layer,
            List<ISetting> allSettings)
        {
            var validationResult = new List<IRuleValidationResult>();

            foreach (var currentModule in layer.Modules)
            {
                var dataElements = currentModule.Value.Cast<ISetting>().ToList();
                foreach (var currentSetting in dataElements)
                {
                    validationResult.AddRange(ValidateSettingItem(allLayers, layer, allSettings, currentModule,
                        currentSetting));
                }
            }

            return validationResult;
        }

        private List<IRuleValidationResult> ValidateSettingItem(List<IHelixLayerInfo> allLayers, IHelixLayerInfo layer,
            List<ISetting> allSettings, KeyValuePair<string, IDataElement[]> currentModule,
            ISetting currentSetting)
        {
            var validationResult = new List<IRuleValidationResult>();

            var success = true;
            foreach (var rule in _rules)
            {
                var result = rule.Validate(allLayers, layer, currentModule, allSettings, currentSetting);
                if (result.Result != RuleResult.Success)
                {
                    validationResult.Add(new RuleValidationResult(result.Result,
                        GetResultMessage(currentSetting, rule, result)));
                    success = false;
                }
            }

            if (success)
            {
                validationResult.Add(new RuleValidationResult(RuleResult.Success,
                    GetSuccessResultMessage(currentSetting)));
            }

            foreach (var child in currentSetting.Children)
            {
                validationResult.AddRange(ValidateSettingItem(allLayers, layer, allSettings, currentModule, child));
            }

            return validationResult;
        }


        private IEnumerable<ISetting> GetAllSettings(Dictionary<string, ISetting[]> templates)
        {
            return templates.Values.SelectMany(p => p.Select(q => q));
        }
    }
}