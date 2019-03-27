namespace Sitecore.Helix.Validator.Common.Data
{
    using System;

    public class RenderingField : IField
    {
        public string Value { get; set; }
        public string Type { get; set; }
        public Guid? BlobId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ITemplate Template { get; set; }
    }
}