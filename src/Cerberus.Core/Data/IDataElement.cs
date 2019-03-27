namespace Sitecore.Helix.Validator.Common.Data
{
    using System;

    public interface IDataElement
    {
        string Path { get; set; }
        Guid Id { get; set; }
        string Name { get; set; }
    }
}