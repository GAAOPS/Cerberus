namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public interface ITemplateRule : IRule
    {
        ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates, List<ITemplate> otherTemplates, ITemplate currentTemplate);
    }
}