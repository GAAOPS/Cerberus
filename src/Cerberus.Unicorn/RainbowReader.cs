namespace Sitecore.Helix.Validator.Unicorn
{
    using System;
    using System.Collections.Generic;
    using Rainbow.Model;

    public abstract class RainbowReader<T>
    {
        protected readonly ISourceDataStore DataStore;
        protected string ReaderStartPath;
        protected HashSet<Guid> ReaderTemplateId;

        protected RainbowReader(ISourceDataStore dataStore)
        {
            DataStore = dataStore;
            ReaderTemplateId = new HashSet<Guid>();
        }

        protected virtual IEnumerable<T> CreateItems(IItemData root)
        {
            var processQueue = new Queue<IItemData>();

            processQueue.Enqueue(root);

            while (processQueue.Count > 0)
            {
                var currentItem = processQueue.Dequeue();

                if (ReaderTemplateId.Contains(currentItem.TemplateId) ||
                    !string.IsNullOrEmpty(ReaderStartPath) && currentItem.Path.StartsWith(ReaderStartPath,
                        StringComparison.InvariantCultureIgnoreCase))

                {
                    yield return CreateSetting(currentItem);
                    continue;
                }

                var children = currentItem.GetChildren();
                foreach (var child in children)
                {
                    processQueue.Enqueue(child);
                }
            }
        }

        protected abstract T CreateSetting(IItemData currentItem);
    }
}