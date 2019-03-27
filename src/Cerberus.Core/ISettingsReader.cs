namespace Sitecore.Helix.Validator.Common
{
    using Data;

    public interface ISettingsReader
    {
        ISetting[] GetSettings(params TreeRoot[] rootPaths);
    }
}