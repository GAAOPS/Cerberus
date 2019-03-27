namespace Sitecore.Helix.Validator.Common.Analyzers
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Data;
    using Rules;

    public class RenderingAnalyzer : Analyzer<IRendering>, IRenderingAnalyzer
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IHelixDependencyConfigurationProvider _dependencyConfigurationProvider;
        private readonly IEnumerable<IRenderingRule> _rules;

        public RenderingAnalyzer(IConfigurationProvider configurationProvider,
            IHelixDependencyConfigurationProvider dependencyConfigurationProvider,
            IEnumerable<IRule> rules)
        {
            _configurationProvider = configurationProvider;
            _dependencyConfigurationProvider = dependencyConfigurationProvider;
            _rules = rules.Cast<IRenderingRule>();
            Name = "Rendering";
        }

        public IAnalyzeResult Analyze()
        {
            var configs = _configurationProvider.Configurations;
            var renderings = _configurationProvider.GetRenderings(configs);
            var allLayers = GetHelixModules(renderings, _dependencyConfigurationProvider.Layers).ToList();
            var validationResult = new List<IRuleValidationResult>();

            foreach (var layer in allLayers)
            {
                validationResult.AddRange(ProcessLayer(layer, allLayers));
            }

            return new AnalyzeResult(validationResult);
        }

        private List<IRuleValidationResult> ProcessLayer(IHelixLayerInfo layer, List<IHelixLayerInfo> allLayers)
        {
            var validationResult = new List<IRuleValidationResult>();

            foreach (var currentModule in layer.Modules)
            {
                var dataElements = currentModule.Value.Cast<IRendering>().ToList();
                foreach (var rendering in dataElements)
                {
                    var success = true;
                    foreach (var rule in _rules)
                    {
                        var result = rule.Validate(allLayers, layer, currentModule, rendering);
                        if (result.Result != RuleResult.Success)
                        {
                            validationResult.Add(new RuleValidationResult(result.Result,
                                GetResultMessage(rendering, rule, result)));
                            success = false;
                        }
                    }

                    if (success)
                    {
                        validationResult.Add(new RuleValidationResult(RuleResult.Success,
                            GetSuccessResultMessage(rendering)));
                    }
                }
            }

            return validationResult;
        }
    }
}