namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Data;

    public class TemplateCircularReference : RuleWithErrorAction, ITemplateRule
    {
        public TemplateCircularReference(ErrorAction errorAction) : base(errorAction)
        {
        }

        public ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates, ITemplate currentTemplate)
        {
            var hasCircularReference = currentTemplate.BaseTemplates.Any(p => p.Equals(currentTemplate.Id));
            var result = GetFailResult();
            return !hasCircularReference
                ? new TemplateRuleValidationResult(RuleResult.Success)
                : new TemplateRuleValidationResult(result,
                    $"The template {currentTemplate} has circular reference.");
        }
    }
}