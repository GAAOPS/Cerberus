namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class TemplateInheritance : RuleWithErrorAction, ITemplateRule
    {
        private readonly bool _allowInheritanceInsideSameModule;

        public TemplateInheritance(bool allowInheritanceInsideSameModule, ErrorAction errorAction) : base(errorAction)
        {
            _allowInheritanceInsideSameModule = allowInheritanceInsideSameModule;
        }

        public IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
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
                        return new RuleValidationResult(GetFailResult(),
                            $"Cross module template inheritance found between {currentTemplate} and {otherTemplates.FirstOrDefault(p => p.Id.Equals(conflictingTemplate.Id))}");
                    }

                    if (!_allowInheritanceInsideSameModule)
                    {
                        return new RuleValidationResult(GetFailResult(),
                            $"Template inheritance found between {currentTemplate} and {currentModule.Value.FirstOrDefault(p => p.Id.Equals(conflictingTemplate.Id))}");
                    }
                }
            }

            return new RuleValidationResult(RuleResult.Success);
        }
    }
}