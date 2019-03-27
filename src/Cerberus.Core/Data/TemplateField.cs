namespace Sitecore.Helix.Validator.Common.Data
{
    using System;

    public class TemplateField : IField
    {
        public string DisplayName { get; set; }

        public string Path { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string Section { get; set; }

        public int SortOrder { get; set; }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public ITemplate Template { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Id: {Id}, Path:{Path}";
        }
    }
}