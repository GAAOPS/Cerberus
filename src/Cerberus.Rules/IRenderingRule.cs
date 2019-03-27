namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using Configuration;
    using Data;

    public interface IRenderingRule : IRule
    {
        IRenderingRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, IRendering currentRendering);
    }
}