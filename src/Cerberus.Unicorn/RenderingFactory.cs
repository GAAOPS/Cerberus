namespace Sitecore.Helix.Validator.Unicorn
{
    using System.Collections.Generic;
    using Common.Data;
    using Rainbow.Model;

    public class RenderingFactory
    {
        protected RenderingFactory()
        {
        }

        public static IRendering Create(IItemData item)
        {
            return new Rendering
            {
                Id = item.Id,
                Name = item.Name,
                Fields = GetRenderingFields(item),
                Path = item.Path
            };
        }

        private static RenderingField[] GetRenderingFields(IItemData currentItem)
        {
            var results = new List<RenderingField>();
            foreach (var sharedField in currentItem.SharedFields)
            {
                results.Add(GetRenderingField(sharedField));
            }

            foreach (var unversionedFields in currentItem.UnversionedFields)
            {
                foreach (var ufield in unversionedFields.Fields)
                {
                    results.Add(GetRenderingField(ufield));
                }
            }

            foreach (var versions in currentItem.Versions)
            {
                foreach (var field in versions.Fields)
                {
                    results.Add(GetRenderingField(field));
                }
            }

            return results.ToArray();
        }

        private static RenderingField GetRenderingField(IItemFieldValue field)
        {
            return new RenderingField
            {
                Id = field.FieldId,
                Name = field.NameHint,
                Value = field.Value,
                Type = field.FieldType,
                BlobId = field.BlobId
            };
        }
    }
}