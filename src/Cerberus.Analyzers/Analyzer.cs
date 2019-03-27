namespace Sitecore.Helix.Validator.Common.Analyzers
{
    using System.Collections.Generic;
    using Configuration;
    using Data;
    using Rules;

    public class Analyzer<T> where T : IDataElement
    {
        public string Name { get; protected set; }

        protected virtual List<IHelixLayerInfo> GetHelixModules(Dictionary<string, T[]> dataItem,
            IList<IHelixLayer> layers)
        {
            var modules = new List<IHelixLayerInfo>();
            foreach (var helixLayer in layers)
            {
                modules.Add(new HelixLayerInfo(helixLayer));
            }

            foreach (var module in dataItem)
            {
                var layer = modules.Find(p => p.Name.Equals(module.Key.Split('.')[0]));
                layer?.Modules.Add(module.Key, module.Value as IDataElement[]);
            }

            return modules;
        }

        protected virtual string GetSuccessResultMessage(IDataElement template)
        {
            return $"{Name} {template} validated successfully.";
        }
        protected virtual string GetResultMessage(IDataElement template, IRule rule, IRuleValidationResult result)
        {
            return $"[{rule.GetType().Name}] {result.Message}";
        }
    }
}