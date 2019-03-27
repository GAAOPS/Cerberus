namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Data;

    public class TemplateInheritance : RuleWithErrorAction, ITemplateRule
    {
        private readonly bool _allowInheritanceInsideSameModule;

        public TemplateInheritance(bool allowInheritanceInsideSameModule, ErrorAction errorAction) : base(errorAction)
        {
            _allowInheritanceInsideSameModule = allowInheritanceInsideSameModule;
        }

        public ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates, ITemplate currentTemplate)
        {
            foreach (var baseTemplate in currentTemplate.BaseTemplates)
            {
                var conflictingTemplates = otherTemplates.Where(o => o.Id.Equals(baseTemplate)).ToList();
                foreach (var conflictingTemplate in conflictingTemplates)
                {
                    var isInSameModule = currentModule.Value.Any(p => p.Id.Equals(conflictingTemplate.Id));
                    if (!isInSameModule)
                    {
                        return new TemplateRuleValidationResult(GetFailResult(),
                            $"Cross module template inheritance found between {currentTemplate} and {otherTemplates.FirstOrDefault(p => p.Id.Equals(conflictingTemplate.Id))}");
                    }

                    if (!_allowInheritanceInsideSameModule)
                    {
                        return new TemplateRuleValidationResult(GetFailResult(),
                            $"Template inheritance found between {currentTemplate} and {currentModule.Value.FirstOrDefault(p => p.Id.Equals(conflictingTemplate.Id))}");
                    }
                }
            }

            return new TemplateRuleValidationResult(RuleResult.Success);
        }
    }
}