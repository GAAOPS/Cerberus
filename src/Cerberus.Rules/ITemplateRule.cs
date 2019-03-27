namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using Configuration;
    using Data;

    public interface ITemplateRule : IRule
    {
        ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, List<IHelixLayerInfo> otherModule,
            List<ITemplate> allTemplates, List<ITemplate> otherTemplates, ITemplate currentTemplate);
    }
}