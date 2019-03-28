namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;
    using Data;

    public interface IConfigurationProvider
    {
        IConfiguration[] Configurations { get; }
        Dictionary<string, IRendering[]> GetRenderings(IConfiguration[] configurations);
        Dictionary<string, ITemplate[]> GetTemplates(IConfiguration[] configurations);
        Dictionary<string, ISetting[]> GetSettings(IConfiguration[] configurations);
    }
}