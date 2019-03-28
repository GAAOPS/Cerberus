namespace Cerberus.Core.Data
{
    using System;
    using System.Collections.Generic;

    public class Setting : BaseItem, ISetting
    {
        public Setting()
        {
            Children = new List<ISetting>();
        }

        public List<ISetting> Children { get; set; }
        public Guid TemplateId { get; set; }
        public ContentField[] Fields { get; set; }
        public ITemplate Template { get; set; }
    }
}