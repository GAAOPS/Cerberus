namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class TemplateNamingConvention : RuleWithErrorAction, ITemplateRule
    {
        private readonly string _illegalChars;

        public TemplateNamingConvention(string illegalChars, ErrorAction errorAction) : base(errorAction)
        {
            _illegalChars = illegalChars;
        }

        public ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule,
            List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates,
            List<ITemplate> otherTemplates,
            ITemplate currentTemplate)
        {
            if (_illegalChars != null)
            {
                var illegalChar = _illegalChars.ToCharArray();
                foreach (var c in illegalChar)
                {
                    if (currentTemplate.Name.IndexOf(c) > -1)
                    {
                        return new TemplateRuleValidationResult(GetFailResult(),
                            $"The Template {currentTemplate} has illegal Character on its name.");
                    }
                }
            }

            return new TemplateRuleValidationResult(RuleResult.Success);
        }
    }
}