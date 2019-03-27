namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class TemplateDuplicateFieldAnalyzer : RuleWithErrorAction, ITemplateRule
    {
        public TemplateDuplicateFieldAnalyzer(ErrorAction errorAction) : base(errorAction)
        {
        }

        public ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates, ITemplate currentTemplate)
        {
            var fields = new HashSet<IField>();
            var result = AddField(currentTemplate, fields, currentTemplate);
            if (result.Result != RuleResult.Success)
            {
                return result;
            }

            foreach (var baseTemplate in currentTemplate.BaseTemplates)
            {
                var bases = allTemplates.FirstOrDefault(p => p.Id.Equals(baseTemplate));
                if (bases != null)
                {
                    result = AddField(bases, fields, currentTemplate);
                    if (result.Result != RuleResult.Success)
                    {
                        return result;
                    }
                }
            }

            return new TemplateRuleValidationResult(RuleResult.Success);
        }

        private ITemplateRuleValidationResult AddField(ITemplate currentTemplate, HashSet<IField> fields,
            ITemplate baseTemplate)
        {
            foreach (var fld in currentTemplate.Fields)
            {
                if (fields.Any(p => p.Name.Equals(fld.Name)))
                {
                    return new TemplateRuleValidationResult(GetFailResult(),
                        $"Duplicate field name '{fld.Name}' detected between {currentTemplate} and {baseTemplate} .");
                }

                fields.Add(fld);
            }

            return new TemplateRuleValidationResult(RuleResult.Success);
        }
    }
}