namespace Sitecore.Helix.Validator.Unicorn
{
    using Common;

    public class PresetTreeRoot : TreeRoot
    {
        public PresetTreeRoot(string name, string path, string databaseName) : base(name, path, databaseName)
        {
            if (name == null)
            {
                Name = path.Substring(path.LastIndexOf('/') + 1);
            }
        }
    }
}