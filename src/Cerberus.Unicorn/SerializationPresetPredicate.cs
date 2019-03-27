namespace Sitecore.Helix.Validator.Unicorn
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml;
    using Common;
    using Common.Configuration;
    using Common.Diagnostics;
    using Rainbow.Model;

    public class SerializationPresetPredicate : IPredicate, ITreeRootFactory
    {
        private readonly IList<PresetTreeRoot> _includeEntries;

        public SerializationPresetPredicate(XmlNode configNode, IConfiguration configuration)
        {
            Assert.ArgumentNotNull(configNode, "configNode");

            _includeEntries = ParsePreset(configNode, configuration?.Name);

            EnsureEntriesExist(configuration?.Name ?? "Unknown");
        }

        public PredicateResult Includes(IItemData itemData)
        {
            Assert.ArgumentNotNull(itemData, "itemData");

            var result = new PredicateResult(true);

            PredicateResult priorityResult = null;

            foreach (var entry in _includeEntries)
            {
                result = Includes(entry, itemData);

                if (result.IsIncluded)
                {
                    result.PredicateComponentId = entry.Name;

                    return result; // it's definitely included if anything includes it
                }

                if (!string.IsNullOrEmpty(result.Justification))
                {
                    priorityResult =
                        result; // a justification means this is probably a more 'important' fail than others
                }
            }

            return priorityResult ?? result; // return the last failure
        }

        public TreeRoot[] GetRootPaths()
        {
            return _includeEntries.ToArray<TreeRoot>();
        }

        [ExcludeFromCodeCoverage] public string FriendlyName => "Serialization Preset Predicate";

        [ExcludeFromCodeCoverage]
        public string Description => "Defines what to include in Unicorn based on XML configuration entries.";

        [ExcludeFromCodeCoverage]
        public KeyValuePair<string, string>[] GetConfigurationDetails()
        {
            var configs = new Collection<KeyValuePair<string, string>>();
            foreach (var entry in _includeEntries)
            {
                var basePath = entry.DatabaseName + ":" + entry.Path;

                configs.Add(new KeyValuePair<string, string>(entry.Name, basePath));
            }

            return configs.ToArray();
        }

        IEnumerable<TreeRoot> ITreeRootFactory.CreateTreeRoots()
        {
            return GetRootPaths();
        }

        /// <summary>
        ///     Checks if a preset includes a given item
        /// </summary>
        protected PredicateResult Includes(PresetTreeRoot entry, IItemData itemData)
        {
            // check for db match
            if (!itemData.DatabaseName.Equals(entry.DatabaseName, StringComparison.OrdinalIgnoreCase))
            {
                return new PredicateResult(false);
            }

            // check for path match
            var unescapedPath = entry.Path.Replace(@"\*", "*");
            if (!itemData.Path.StartsWith(unescapedPath + "/", StringComparison.OrdinalIgnoreCase) &&
                !itemData.Path.Equals(unescapedPath, StringComparison.OrdinalIgnoreCase))
            {
                return new PredicateResult(false);
            }

            // check excludes
            return ExcludeMatches(entry, itemData);
        }

        protected virtual PredicateResult ExcludeMatches(PresetTreeRoot entry, IItemData itemData)
        {
            return new PredicateResult(true);
        }


        private IList<PresetTreeRoot> ParsePreset(XmlNode configuration, string configurationName)
        {
            var presets = configuration.ChildNodes
                .Cast<XmlNode>()
                .Where(node => node.Name == "include")
                .Select(CreateIncludeEntry)
                .ToList();

            var names = new HashSet<string>();
            foreach (var preset in presets)
            {
                if (!names.Contains(preset.Name))
                {
                    names.Add(preset.Name);
                    continue;
                }

                throw new InvalidOperationException(
                    $"Multiple predicate include nodes in configuration '{configurationName}' had the same name '{preset.Name}'. This is not allowed. Note that this can occur if you did not specify the name attribute and two include entries end in an item with the same name. Use the name attribute on the include tag to give a unique name.");
            }

            return presets;
        }

        private void EnsureEntriesExist(string configurationName)
        {
            // no entries = throw!
            if (_includeEntries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No include entries were present on the predicate for the {configurationName} Unicorn configuration. You must explicitly specify the items you want to include.");
            }
        }


        protected virtual PresetTreeRoot CreateIncludeEntry(XmlNode configuration)
        {
            var database = GetExpectedAttribute(configuration, "database");
            var path = GetExpectedAttribute(configuration, "path");

            // ReSharper disable once PossibleNullReferenceException
            var name = configuration.Attributes["name"];
            var nameValue = name == null ? path.Substring(path.LastIndexOf('/') + 1) : name.Value;

            var root = new PresetTreeRoot(nameValue, path, database);

            return root;
        }


        private static string GetExpectedAttribute(XmlNode node, string attributeName)
        {
            // ReSharper disable once PossibleNullReferenceException
            var attribute = node.Attributes[attributeName];

            if (attribute == null)
            {
                throw new InvalidOperationException(
                    $"Missing expected '{attributeName}' attribute on '{node.Name}' node while processing predicate: {node.OuterXml}");
            }

            return attribute.Value;
        }
    }
}