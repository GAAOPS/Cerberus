namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class RenderingNameConvention : RuleWithErrorAction, IRenderingRule
    {
        public RenderingNameConvention(ErrorAction errorAction) : base(errorAction)
        {
        }

        public IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
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
                    return new RuleValidationResult(GetFailResult(),
                        $"The Rendering {currentRendering} has illegal Character on its name.");
                }
            }

            return new RuleValidationResult(RuleResult.Success);
        }
    }
}