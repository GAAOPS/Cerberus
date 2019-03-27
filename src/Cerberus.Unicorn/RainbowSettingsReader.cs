namespace Sitecore.Helix.Validator.Unicorn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Common.Configuration;
    using Common.Data;
    using Rainbow.Model;

    public class RainbowSettingsReader : RainbowReader<ISetting>, ISettingsReader
    {
        private readonly IConfigurationProvider _configurationProvider;
        private List<ITemplate> _allTemplates;

        public RainbowSettingsReader(ISourceDataStore dataStore, string settingsRootPath,
            IConfigurationProvider configurationProvider) : base(dataStore)
        {
            _configurationProvider = configurationProvider;
            ReaderStartPath = settingsRootPath;
        }

        private List<ITemplate> AllTemplates => _allTemplates ??
                                                (_allTemplates = _configurationProvider
                                                    .GetTemplates(_configurationProvider.Configurations)
                                                    .SelectMany(p => p.Value).ToList());


        public ISetting[] GetSettings(TreeRoot[] rootPaths)
        {
            return rootPaths
                .AsParallel()
                .SelectMany(root =>
                {
                    var rootItem = DataStore.InnerDataStore.GetByPath(root.Path, root.DatabaseName);

                    if (rootItem == null)
                    {
                        return Enumerable.Empty<ISetting>();
                    }

                    return rootItem.SelectMany(CreateItems);
                })
                .ToArray();
        }

        protected override ISetting CreateSetting(IItemData currentItem)
        {
            if (currentItem == null)
            {
                throw new ArgumentException("Setting item passed to parse was null", nameof(currentItem));
            }

            if (!currentItem.Path.StartsWith(ReaderStartPath))
            {
                throw new ArgumentException("Setting item passed to parse was not a Setting item",
                    nameof(currentItem));
            }

            var result = SettingsFactory.Create(currentItem, AllTemplates);

            return result;
        }
    }
}