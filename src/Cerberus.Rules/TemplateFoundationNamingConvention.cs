namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Data;

    public class TemplateFoundationNamingConvention : RuleWithErrorAction, ITemplateRule
    {
        public TemplateFoundationNamingConvention(ErrorAction errorAction) : base(errorAction)
        {
        }

        public ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
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
                return new TemplateRuleValidationResult(GetFailResult(),
                    $"The Template {currentTemplate} is not follows Foundation layer naming convention.");
            }

            return new TemplateRuleValidationResult(RuleResult.Success);
        }
    }
}