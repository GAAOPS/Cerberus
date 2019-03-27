namespace Cerberus.Unicorn
{
    using System;
    using System.Linq;
    using Core;
    using Core.Data;
    using Rainbow.Model;

    public class RainbowRenderingReader : RainbowReader<IRendering>, IRenderingReader
    {
        public RainbowRenderingReader(ISourceDataStore dataStore) : base(dataStore)
        {
            // Controller Rendering
            ReaderTemplateId.Add(new Guid("{2A3E91A0-7987-44B5-AB34-35C2D9DE83B9}"));
        }


        public IRendering[] GetRenderings(TreeRoot[] rootPaths)
        {
            return rootPaths
                .AsParallel()
                .SelectMany(root =>
                {
                    var rootItem = DataStore.InnerDataStore.GetByPath(root.Path, root.DatabaseName);

                    if (rootItem == null)
                    {
                        return Enumerable.Empty<IRendering>();
                    }

                    return rootItem.SelectMany(CreateItems);
                })
                .ToArray();
        }


        protected override IRendering CreateSetting(IItemData currentItem)
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

            var result = RenderingFactory.Create(currentItem);

            return result;
        }

    }
}