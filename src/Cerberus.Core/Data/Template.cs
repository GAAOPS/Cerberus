namespace Cerberus.Core.Data
{
    using System;
    using Configuration;

    public class Template : BaseItem, ITemplate
    {
        public IHelixModuleInfo HelixModuleInfo { get; set; }
        public Guid[] BaseTemplates { get; set; }
        public TemplateField[] Fields { get; set; }
    }
}