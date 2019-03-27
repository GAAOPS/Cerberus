namespace Sitecore.Helix.Validator.Common.Data
{
    using System;

    public class Rendering : IRendering
    {
        public string Path { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public RenderingField[] Fields { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Path: {Path}, Id: {Id}";
        }
    }
}