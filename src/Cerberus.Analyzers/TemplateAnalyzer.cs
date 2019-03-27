namespace Sitecore.Helix.Validator.Common.Analyzers
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Data;
    using Rules;

    public class TemplateAnalyzer : Analyzer<ITemplate>, ITemplateAnalyzer
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IHelixDependencyConfigurationProvider _dependencyConfigurationProvider;
        private readonly IEnumerable<ITemplateRule> _rules;

        public TemplateAnalyzer(IConfigurationProvider configurationProvider,
            IHelixDependencyConfigurationProvider dependencyConfigurationProvider,
            IEnumerable<IRule> rules)
        {
            _configurationProvider = configurationProvider;
            _dependencyConfigurationProvider = dependencyConfigurationProvider;
            _rules = rules.Cast<ITemplateRule>();
            Name = "Templates";
        }

        public IAnalyzeResult Analyze()
        {
            var configs = _configurationProvider.Configurations;
            var templates = _configurationProvider.GetTemplates(configs);
            var allLayers = GetHelixModules(templates, _dependencyConfigurationProvider.Layers).ToList();
            var allTemplates = GetAllTemplates(templates).ToList();
            var results = new List<IRuleValidationResult>();

            foreach (var layer in allLayers)
            {
                results.AddRange(ProcessLayer(allLayers, layer, allTemplates));
            }

            return new AnalyzeResult(results);
        }


        private List<IRuleValidationResult> ProcessLayer(List<IHelixLayerInfo> allLayers, IHelixLayerInfo layer,
            List<ITemplate> allTemplates)
        {
            var others = allLayers.Except(layer.DependentLayers, new HelixLayerEqualityComparer())
                .Cast<IHelixLayerInfo>().ToList();
            var otherTemplates = others
                .SelectMany(p => p.Modules.SelectMany(q => q.Value.Select(u => u as ITemplate))).ToList();
            var validationResult = new List<IRuleValidationResult>();
            foreach (var currentModule in layer.Modules)
            {
                var dataElements = currentModule.Value.Cast<ITemplate>().Where(p => p.BaseTemplates.Length > 0)
                    .ToList();
                foreach (var template in dataElements)
                {
                    var success = true;
                    foreach (var rule in _rules)
                    {
                        var result = rule.Validate(allLayers, layer, currentModule, others, allTemplates,
                            otherTemplates, template);
                        if (result.Result != RuleResult.Success)
                        {
                            validationResult.Add(new RuleValidationResult(result.Result,
                                GetResultMessage(template, rule, result)));
                            success = false;
                        }
                    }

                    if (success)
                    {
                        validationResult.Add(new RuleValidationResult(RuleResult.Success,
                            GetSuccessResultMessage(template)));
                    }
                }
            }

            return validationResult;
        }

        private IEnumerable<ITemplate> GetAllTemplates(Dictionary<string, ITemplate[]> templates)
        {
            return templates.Values.SelectMany(p => p.Select(q => q));
        }

    }
}