namespace Cerberus.Core
{
    using Data;

    public interface ISettingsReader
    {
        ISetting[] GetSettings(params TreeRoot[] rootPaths);
    }
}