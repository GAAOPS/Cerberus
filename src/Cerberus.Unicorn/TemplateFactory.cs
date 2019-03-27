namespace Sitecore.Helix.Validator.Unicorn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Configuration;
    using Common.Data;
    using Rainbow.Model;
    using ValGuGuidator.Common.Sc;
    using ValGuidator.Common.Sc;

    public class TemplateFactory
    {
        private static readonly Guid TemplateFieldTemplateId = TemplateGuids.TemplateField;
        private static readonly Guid TemplateSectionTemplateId = TemplateGuids.TemplateSection;
        private static readonly Guid DisplayNameFieldId = FieldGuids.DisplayName;
        private static readonly Guid TemplateFieldTitleFieldId = TemplateFieldGuids.Title;
        private static readonly Guid FieldTypeFieldId = TemplateFieldGuids.Type;
        private static readonly Guid SourceFieldId = TemplateFieldGuids.Source;
        private static readonly Guid SortOrderFieldId = FieldGuids.Sortorder;
        private static readonly Guid BaseTemplateFieldId = FieldGuids.BaseTemplate;
        private static HashSet<Guid> _ignoreList;

        protected TemplateFactory()
        {
        }

        private static ICollection<Guid> IgnoredBaseTemplateIds =>
            _ignoreList ?? (_ignoreList = new HashSet<Guid>
                {TemplateGuids.StandardTemplate, TemplateGuids.Folder});

        public static ITemplate Create(IItemData item, HelixModuleInfo moduleInfo)
        {
            var template = new Template
            {
                Id = item.Id,
                BaseTemplates =
                    GetBaseTemplates(GetFieldValue(item, BaseTemplateFieldId,
                        string.Empty)),
                Name = item.Name,
                Path = item.Path,
                HelixModuleInfo= moduleInfo
            };
            template.Fields = GetTemplateFields(item, template);

            return template;
        }

        private static Guid[] GetBaseTemplates(string value)
        {
            var ignoredIds = IgnoredBaseTemplateIds;

            return GetMultiListValue(value)
                .Where(id => !ignoredIds.Contains(id))
                .ToArray();
        }

        private static Guid[] GetMultiListValue(string value)
        {
            return value.Split('|')
                .Select(item =>
                {
                    if (Guid.TryParse(item, out var result))
                    {
                        return result;
                    }

                    return Guid.Empty;
                })
                .Where(item => item != Guid.Empty)
                .ToArray();
        }

        private static TemplateField[] GetTemplateFields(IItemData item, Template template)
        {
            var results = new List<TemplateField>();

            var sections = item.GetChildren().Where(child => child.TemplateId == TemplateSectionTemplateId);

            foreach (var section in sections)
            {
                var fields = section.GetChildren().Where(child => child.TemplateId == TemplateFieldTemplateId);

                foreach (var field in fields)
                {
                    results.Add(new TemplateField
                    {
                        Id = field.Id,
                        Template = template,
                        DisplayName = GetFieldValue(field, TemplateFieldTitleFieldId, null) ??
                                      GetFieldValue(field, DisplayNameFieldId, string.Empty),
                        Name = field.Name,
                        Path = field.Path,
                        Section = section.Name,
                        SortOrder = GetFieldValueAsInt(field, SortOrderFieldId, 100),
                        Source = GetFieldValue(field, SourceFieldId, string.Empty),
                        Type = GetFieldValue(field, FieldTypeFieldId, string.Empty)
                    });
                }
            }

            return results.ToArray();
        }

        private static string GetFieldValue(IItemData item, Guid fieldId, string defaultValue)
        {
            foreach (var field in item.SharedFields)
            {
                if (field.FieldId == fieldId)
                {
                    return field.Value;
                }
            }

            foreach (var language in item.UnversionedFields)
            {
                foreach (var field in language.Fields)
                {
                    if (field.FieldId == fieldId)
                    {
                        return field.Value;
                    }
                }
            }

            foreach (var version in item.Versions)
            {
                foreach (var field in version.Fields)
                {
                    if (field.FieldId == fieldId)
                    {
                        return field.Value;
                    }
                }
            }

            return defaultValue;
        }

        private static int GetFieldValueAsInt(IItemData item, Guid fieldId, int defaultValue)
        {
            var value = GetFieldValue(item, fieldId, string.Empty);

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}