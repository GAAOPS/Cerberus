namespace Cerberus.Unicorn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Core.Diagnostics;
    using Rainbow;
    using Rainbow.Formatting;
    using Rainbow.Model;
    using Rainbow.Storage;
    using ITreeRootFactory = Core.ITreeRootFactory;
    using TreeRoot = Core.TreeRoot;

    public class SerializationFileSystemDataStore : ISnapshotCapableDataStore, IDocumentable, IDisposable
    {
        private readonly ISerializationFormatter _formatter;
        private readonly ITreeRootFactory _rootFactory;
        private readonly bool _useDataCache;


        protected readonly string PhysicalRootPath;
        protected readonly List<SerializationFileSystemTree> Trees;

        public SerializationFileSystemDataStore(string physicalRootPath, bool useDataCache,
            ITreeRootFactory rootFactory, ISerializationFormatter formatter)
        {
            Assert.ArgumentNotNullOrEmpty(physicalRootPath, nameof(physicalRootPath));
            Assert.ArgumentNotNull(formatter, nameof(formatter));
            Assert.ArgumentNotNull(rootFactory, nameof(rootFactory));

            _useDataCache = useDataCache;
            _rootFactory = rootFactory;
            _formatter = formatter;
            _formatter.ParentDataStore = this;

            PhysicalRootPath = InitRootPath(physicalRootPath);

            Trees = InitTrees(_formatter, useDataCache);
        }

        public string DataSourceLocation => PhysicalRootPath;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual string FriendlyName => "Serialization File System Data Store";

        public virtual string Description =>
            "Stores serialized items on disk using the SFS tree format, where each root is a separate tree.";

        public virtual KeyValuePair<string, string>[] GetConfigurationDetails()
        {
            return new[]
            {
                new KeyValuePair<string, string>("Serialization formatter",
                    DocumentationUtility.GetFriendlyName(_formatter)),
                new KeyValuePair<string, string>("Physical root path", PhysicalRootPath),
                new KeyValuePair<string, string>("Total internal SFS trees", Trees.Count.ToString())
            };
        }

        public virtual IEnumerable<IItemData> GetSnapshot()
        {
            return Trees.SelectMany(tree => tree.GetSnapshot());
        }

        public virtual void Save(IItemData item)
        {
        }

        /*
		Moving an item involves:
		- Get the item from the tree, delete
		- If the new path is included in ANY tree
			- Get the serialized parent at the destination
			- Get the moved tree from Sitecore, save whole tree (NOTE: we had to update to final path in DP - what about children are those with old or new path?)

		Renaming an item involves:
		- The same thing as moving an item
		*/
        public virtual void MoveOrRenameItem(IItemData itemWithFinalPath, string oldPath)
        {
        }

        public virtual IEnumerable<IItemData> GetByPath(string path, string database)
        {
            var tree = GetTreeForPath(path, database);

            if (tree == null)
            {
                return Enumerable.Empty<IItemData>();
            }

            return tree.GetItemsByPath(path);
        }

        public virtual IItemData GetByPathAndId(string path, Guid id, string database)
        {
            Assert.ArgumentNotNullOrEmpty(path, "path");
            Assert.ArgumentNotNullOrEmpty(database, "database");
            Assert.ArgumentCondition(id != default(Guid), "id",
                "The item ID must not be the null guid. Use GetByPath() if you don't know the ID.");

            var items = GetByPath(path, database).ToArray();

            return items.FirstOrDefault(item => item.Id == id);
        }

        public virtual IItemData GetById(Guid id, string database)
        {
            foreach (var tree in Trees)
            {
                var result = tree.GetItemById(id);

                if (result != null && result.DatabaseName.Equals(database))
                {
                    return result;
                }
            }

            return null;
        }

        public virtual IEnumerable<IItemMetadata> GetMetadataByTemplateId(Guid templateId, string database)
        {
            return Trees.Select(tree => tree.GetRootItem())
                .Where(root => root != null)
                .AsParallel()
                .SelectMany(tree => FilterDescendantsAndSelf(tree, item => item.TemplateId == templateId));
        }

        public virtual IEnumerable<IItemData> GetChildren(IItemData parentItem)
        {
            var tree = GetTreeForPath(parentItem.Path, parentItem.DatabaseName);

            if (tree == null)
            {
                throw new InvalidOperationException($"No trees contained the global path {parentItem.Path}");
            }

            return tree.GetChildren(parentItem);
        }

        public void CheckConsistency(string database, bool fixErrors, Action<string> logMessageReceiver)
        {
            throw new NotImplementedException();
        }

        public virtual void ResetTemplateEngine()
        {
            // do nothing, the YAML serializer has no template engine
        }

        public virtual bool Remove(IItemData item)
        {
            return false;
        }

        public virtual void RegisterForChanges(Action<IItemMetadata, string> actionOnChange)
        {
        }

        public virtual void Clear()
        {
            // since we're tearing everything down we dispose all existing trees, watchers, etc and start over
            foreach (var tree in Trees)
            {
                tree.Dispose();
            }

            Trees.Clear();

            ActionRetryer.Perform(ClearAllFiles);

            // bring the trees back up, which will reestablish watchers and such
            Trees.AddRange(InitializeTrees(_formatter, _useDataCache));
        }

        protected virtual void ClearAllFiles()
        {
        }

        protected virtual string InitializeRootPath(string rootPath)
        {
            return InitRootPath(rootPath);
        }

        private string InitRootPath(string rootPath)
        {
            if (rootPath.StartsWith("~") || rootPath.StartsWith("/"))
            {
                var cleanRootPath =
                    rootPath.TrimStart('~', Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                cleanRootPath = cleanRootPath.Replace("/", Path.DirectorySeparatorChar.ToString());

                var basePath = HostingEnvironment.IsHosted
                    ? HostingEnvironment.MapPath("~/")
                    : AppDomain.CurrentDomain.BaseDirectory;
                rootPath = Path.Combine(basePath ?? throw new InvalidOperationException("base path not found"),
                    cleanRootPath);
            }

            // convert root path to canonical form, so subsequent transformations can do string comparison
            // http://stackoverflow.com/questions/970911/net-remove-dots-from-the-path
            if (rootPath.Contains(".."))
            {
                rootPath = Path.GetFullPath(rootPath);
            }

            if (!Directory.Exists(rootPath))
            {
                throw new InvalidOperationException($"Path not found: {rootPath}");
            }

            return rootPath;
        }

        protected virtual SerializationFileSystemTree GetTreeForPath(string path, string database)
        {
            SerializationFileSystemTree foundTree = null;
            foreach (var tree in Trees)
            {
                if (!tree.DatabaseName.Equals(database, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!tree.ContainsPath(path))
                {
                    continue;
                }

                if (foundTree != null)
                {
                    throw new InvalidOperationException(
                        $"The trees {foundTree.Name} and {tree.Name} both contained the global path {path} - overlapping trees are not allowed.");
                }

                foundTree = tree;
            }

            return foundTree;
        }

        // note: we pass in these params (formatter, datacache) so that overriding classes may get access to private vars indirectly (can't get at them otherwise because this is called from the constructor)
        protected virtual List<SerializationFileSystemTree> InitializeTrees(ISerializationFormatter formatter,
            bool useDataCache)
        {
            return InitTrees(formatter, useDataCache);
        }

        private List<SerializationFileSystemTree> InitTrees(ISerializationFormatter formatter, bool useDataCache)
        {
            var result = new List<SerializationFileSystemTree>();
            var roots = _rootFactory.CreateTreeRoots();

            foreach (var root in roots)
            {
                result.Add(CreateTree(root, formatter, useDataCache));
            }

            return result;
        }

        // note: we pass in these params (formatter, datacache) so that overriding classes may get access to private vars indirectly (can't get at them otherwise because this is called from the constructor)
        protected virtual SerializationFileSystemTree CreateTree(TreeRoot root, ISerializationFormatter formatter,
            bool useDataCache)
        {
            var tree = new SerializationFileSystemTree(root.Name, root.Path, root.DatabaseName,
                Path.Combine(PhysicalRootPath, root.Name), formatter, useDataCache);

            return tree;
        }

        protected virtual IList<IItemMetadata> FilterDescendantsAndSelf(IItemData root,
            Func<IItemMetadata, bool> predicate)
        {
            Assert.ArgumentNotNull(root, "root");

            var descendants = new List<IItemMetadata>();

            var childQueue = new Queue<IItemMetadata>();
            childQueue.Enqueue(root);

            while (childQueue.Count > 0)
            {
                var parent = childQueue.Dequeue();

                if (predicate(parent))
                {
                    descendants.Add(parent);
                }

                var tree = GetTreeForPath(parent.Path, root.DatabaseName);

                if (tree == null)
                {
                    continue;
                }

                var children = tree.GetChildrenMetadata(parent).ToArray();

                foreach (var item in children)
                {
                    childQueue.Enqueue(item);
                }
            }

            return descendants;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var tree in Trees)
                {
                    tree.Dispose();
                }
            }
        }
    }
}