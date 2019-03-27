namespace Sitecore.Helix.Validator.Unicorn
{
    using System;
    using System.Linq;
    using Common;
    using Common.Configuration;
    using Common.Data;
    using Rainbow.Model;
    using ValGuidator.Common.Sc;

    public class RainbowTemplateReader : RainbowReader<ITemplate>, ITemplateReader
    {
        private readonly IHelixModuleProvider _helixModuleProvider;

        public RainbowTemplateReader(ISourceDataStore dataStore, IHelixModuleProvider helixModuleProvider) : base(dataStore)
        {
            _helixModuleProvider = helixModuleProvider;
            ReaderTemplateId.Add(TemplateGuids.Template);
        }


        public ITemplate[] GetTemplates(TreeRoot[] rootPaths)
        {
            return rootPaths
                .AsParallel()
                .SelectMany(root =>
                {
                    var rootItem = DataStore.InnerDataStore.GetByPath(root.Path, root.DatabaseName);

                    if (rootItem == null)
                    {
                        return Enumerable.Empty<ITemplate>();
                    }

                    return rootItem.SelectMany(CreateItems);
                })
                .ToArray();
        }

        protected override ITemplate CreateSetting(IItemData currentItem)
        {
            if (currentItem == null)
            {
                throw new ArgumentException("Template item passed to parse was null", nameof(currentItem));
            }

            if (!ReaderTemplateId.Contains(currentItem.TemplateId))
            {
                throw new ArgumentException("Template item passed to parse was not a Template item",
                    nameof(currentItem));
            }

            var moduleInfo = _helixModuleProvider.GetModuleLayerByPath(currentItem.SerializedItemId);
            var result = TemplateFactory.Create(currentItem,moduleInfo);

            return result;
        }
    }
}