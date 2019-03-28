namespace Cerberus.Rules
{
    using System;
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class TemplateFoundationNamingConvention : RuleWithErrorAction, ITemplateRule
    {
        public TemplateFoundationNamingConvention(ErrorAction errorAction) : base(errorAction)
        {
        }

        public IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule,
            List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates,
            ITemplate currentTemplate)
        {
            if (currentLayer.Name.Equals("Foundation", StringComparison.InvariantCultureIgnoreCase) &&
                !currentTemplate.Name.StartsWith("_"))
            {
                return new RuleValidationResult(GetFailResult(),
                    $"The Template {currentTemplate} is not follows Foundation layer naming convention.");
            }

            return new RuleValidationResult(RuleResult.Success);
        }
    }
}