namespace Sitecore.Helix.Validator.Common.Analyzers.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Data;

    public class RenderingDataSource : RuleWithErrorAction, IRenderingRule
    {
        private readonly bool _datasourceLocation;
        private readonly bool _datasourceTemplate;
        private readonly Guid _renderingDatasourceLocationId = new Guid("{B5B27AF1-25EF-405C-87CE-369B3A004016}");
        private readonly Guid _renderingDatasourceTemplateId = new Guid("{1A7C85E5-DC0B-490D-9187-BB1DBCB4C72F}");

        public RenderingDataSource(bool datasourceLocation, bool datasourceTemplate, ErrorAction errorAction) : base(
            errorAction)
        {
            _datasourceLocation = datasourceLocation;
            _datasourceTemplate = datasourceTemplate;
        }

        public IRenderingRuleValidationResult Validate(IEnumerable<IHelixLayerInfo> allLayers,
            IHelixLayerInfo currentLayer, KeyValuePair<string, IDataElement[]> currentModule,
            IRendering currentRendering)
        {
            var datasourceLocation =
                currentRendering.Fields.FirstOrDefault(p => p.Id == _renderingDatasourceLocationId);
            var datasourceTemplate =
                currentRendering.Fields.FirstOrDefault(p => p.Id == _renderingDatasourceTemplateId);

            if (_datasourceLocation && datasourceLocation != null && !string.IsNullOrEmpty(datasourceLocation.Value) &&
                datasourceLocation.Value.StartsWith("/"))
            {
                return new RenderingRuleValidationResult(GetFailResult(),
                    $"DataSource Location is set to: {datasourceLocation.Value} for this rendering: {currentRendering}");
            }

            if (_datasourceTemplate && datasourceTemplate != null && !string.IsNullOrEmpty(datasourceTemplate.Value))
            {
                return new RenderingRuleValidationResult(GetFailResult(),
                    $"DataSource Template is set to: {datasourceTemplate.Value} for this rendering: {currentRendering}");
            }

            return new RenderingRuleValidationResult(RuleResult.Success);
        }
    }
}