namespace Cerberus.Core.Data
{
    using System;
    using Configuration;

    public class Template : BaseItem, ITemplate
    {
        public Guid[] BaseTemplates { get; set; }
        public TemplateField[] Fields { get; set; }
        public IHelixModuleInfo HelixModuleInfo { get; set; }
    }
}