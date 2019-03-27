namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using Configuration;
    using Data;

    public class RenderingNameConvention : RuleWithErrorAction, IRenderingRule
    {
        public RenderingNameConvention(ErrorAction errorAction) : base(errorAction)
        {
        }

        public IRenderingRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer, KeyValuePair<string, IDataElement[]> currentModule,
            IRendering currentRendering)
        {
            var illegalChar = new List<string>
            {
                ",", "-", "."
            };
            foreach (var c in illegalChar)
            {
                if (currentRendering.Name.Contains("-"))
                {
                    return new RenderingRuleValidationResult(GetFailResult(),
                        $"The Rendering {currentRendering} has illegal Character on its name.");
                }
            }

            return new RenderingRuleValidationResult(RuleResult.Success);
        }
    }
}