namespace Cerberus.Core.Data
{
    using System;
    using System.Collections.Generic;

    public interface ISetting : IDataElement
    {
        List<ISetting> Children { get; set; }
        Guid TemplateId { get; set; }
        ContentField[] Fields { get; set; }
        ITemplate Template { get; set; }
    }
}