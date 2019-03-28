namespace Cerberus.Core
{
    using Data;

    public interface ITemplateReader
    {
        ITemplate[] GetTemplates(params TreeRoot[] rootPaths);
    }
}