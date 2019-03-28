namespace Cerberus.Core.Data
{
    using System;

    public interface ITemplate : IDataElement
    {
        Guid[] BaseTemplates { get; set; }
        TemplateField[] Fields { get; set; }
    }
}