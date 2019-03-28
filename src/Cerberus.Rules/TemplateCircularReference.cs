namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class TemplateCircularReference : RuleWithErrorAction, ITemplateRule
    {
        public TemplateCircularReference(ErrorAction errorAction) : base(errorAction)
        {
        }

        public IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates, ITemplate currentTemplate)
        {
            var hasCircularReference = currentTemplate.BaseTemplates.Any(p => p.Equals(currentTemplate.Id));
            var result = GetFailResult();
            return !hasCircularReference
                ? new RuleValidationResult(RuleResult.Success)
                : new RuleValidationResult(result,
                    $"The template {currentTemplate} has circular reference.");
        }
    }
}