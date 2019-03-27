namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System.Collections.Generic;
    using Configuration;
    using Data;

    public interface ISettingRule : IRule
    {
        ITemplateRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers, IHelixLayerInfo currentLayer,
            KeyValuePair<string, IDataElement[]> currentModule, IEnumerable<ISetting> allSettings,
            ISetting currentSetting);
    }
}