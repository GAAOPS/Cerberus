namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public interface IRenderingRule : IRule
    {
        IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, IRendering currentRendering);
    }
}