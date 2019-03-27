namespace Cerberus.Rules
{
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public interface ISettingRule : IRule
    {
        ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, IEnumerable<ISetting> allSettings,
            ISetting currentSetting);
    }
}