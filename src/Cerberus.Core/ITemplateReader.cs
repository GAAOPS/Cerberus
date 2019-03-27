namespace Sitecore.Helix.Validator.Common
{
    using Data;

    public interface ITemplateReader
    {
        ITemplate[] GetTemplates(params TreeRoot[] rootPaths);
    }
}