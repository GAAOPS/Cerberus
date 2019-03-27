namespace Sitecore.Helix.Validator.Unicorn
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Data;
    using Rainbow.Model;

    public static class SettingsFactory
    {
        public static ISetting Create(IItemData item, List<ITemplate> allTemplates)
        {
            var setting = new Setting
            {
                Id = item.Id,
                TemplateId = item.TemplateId,
                Name = item.Name,
                Template = GetTemplate(item, allTemplates),
                Path = item.Path,
                Fields = GetFields(item)
            };

            foreach (var itemData in item.GetChildren().ToList())
            {
                setting.Children.Add(Create(itemData, allTemplates));
            }

            return setting;
        }

        private static ITemplate GetTemplate(IItemData item, List<ITemplate> allTemplates)
        {
            return allTemplates.FirstOrDefault(q => q.Id.Equals(item.TemplateId));
        }

        private static ContentField[] GetFields(IItemData currentItem)
        {
            var results = new List<ContentField>();
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

        private static ContentField GetRenderingField(IItemFieldValue field)
        {
            return new ContentField
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