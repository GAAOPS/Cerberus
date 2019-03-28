namespace Cerberus.Rules
{
    using System;
    using System.Collections.Generic;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Data;

    public class SettingCrossModuleTemplateCheck : RuleWithErrorAction, ISettingRule
    {
        //TODO: This rule is just a sketch for testing the analyzer. It needs a rewrite
        public SettingCrossModuleTemplateCheck(ErrorAction errorAction) : base(errorAction)
        {
        }

        public IRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer, KeyValuePair<string, IDataElement[]> currentModule,
            IEnumerable<ISetting> allSettings, ISetting currentSetting)
        {
            if (currentSetting.Template != null &&
                currentSetting.Template.Path.IndexOf(currentLayer.Name, StringComparison.InvariantCultureIgnoreCase) <
                0 && !TemplateFromFoundation(currentSetting))
            {
                return new RuleValidationResult(GetFailResult(),
                    $"Cross module template inheritance found for setting: {currentSetting} with the template: {currentSetting.Template}");
            }

            return new RuleValidationResult(RuleResult.Success);
        }

        private bool TemplateFromFoundation(ISetting currentSetting)
        {
            return currentSetting.Template.Path.IndexOf("Foundation", StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}