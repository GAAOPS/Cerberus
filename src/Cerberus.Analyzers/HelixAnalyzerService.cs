namespace Cerberus.Analyzers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Configy;
    using Configy.Containers;
    using Configy.Parsing;
    using Core.Analyzers;
    using Core.Analyzers.Rules;
    using Core.Configuration;
    using Core.Logging;

    public sealed class HelixAnalyzerService : XmlContainerBuilder, IHelixAnalyzerService
    {
        private readonly IDataSourceLocation _dataSourceLocation;
        private readonly IContainer _logProvider;
        private List<IHelixAnalyzer> _analyzers;

        public HelixAnalyzerService(XmlNode configNode, IDataSourceLocation dataSourceLocation,
            ICerberusVariablesReplacer variablesReplacer) : base(
            variablesReplacer)
        {
            _dataSourceLocation = dataSourceLocation;
            if (configNode == null)
            {
                return;
            }

            var analyzers = GetAnalyzers(configNode, out var provider);
            _logProvider = GetLogContainer(configNode);

            foreach (var node in analyzers)
            {
                var ruleProvider = RegisterRules(node);
                var type = XmlActivator.GetType(node);
                if (type != null)
                {
                    provider.Register(type,
                        () => XmlActivator.CreateInstance(type, node, new[] {provider, ruleProvider, _logProvider}),
                        true);

                    if (provider.Resolve(type) is IHelixAnalyzer resolve)
                    {
                        Analyzers.Add(resolve);
                    }
                }
            }
        }

        public IList<IHelixAnalyzer> Analyzers => _analyzers ?? (_analyzers = new List<IHelixAnalyzer>());

        public IAnalyzeResult Analyze()
        {
            var result = new List<IRuleValidationResult>();
            var logger = _logProvider.Resolve<ILogDispatcher>();
            foreach (var helixAnalyzer in Analyzers)
            {
                var analyzeResult = helixAnalyzer.Analyze();
                result.AddRange(analyzeResult.Results);
                LogResult(logger, helixAnalyzer, analyzeResult);
            }

            logger.Report();

            return new AnalyzeResult(result);
        }

        private void LogResult(ILogDispatcher logger, IHelixAnalyzer helixAnalyzer, IAnalyzeResult analyzeResult)
        {
            logger.InitLogger(helixAnalyzer.Name, "");
            foreach (var result in analyzeResult.Results)
            {
                logger.Log(result.Message, result.Result.ToLogLevel());
            }
        }

        private IContainer GetLogContainer(XmlNode configNode)
        {
            var ioc = CreateSingleNodeContainer(configNode, "loggers");
            ioc.Register(typeof(IDataSourceLocation), () => _dataSourceLocation, true);
            return ioc;
        }

        private IContainer RegisterRules(XmlElement configNode)
        {
            var rules = configNode.ChildNodes.OfType<XmlElement>()
                .Where(nd => nd.NodeType == XmlNodeType.Element && nd.Name.Equals("rule"));
            var provider = new MicroContainer("rules", "");
            var ruleInstances = new List<IRule>();

            foreach (var xRule in rules)
            {
                var ruleType = XmlActivator.GetType(xRule);
                var ruleProvider = CreateSingleNodeContainer(xRule, "rule");
                ruleInstances.Add(XmlActivator.CreateInstance(ruleType, xRule, new[] {ruleProvider}) as IRule);
            }

            provider.Register(typeof(IEnumerable<IRule>), () => ruleInstances, true);

            return provider;
        }

        private IEnumerable<XmlElement> GetAnalyzers(XmlNode configNode, out IContainer provider)
        {
            var analyzers = configNode.ChildNodes.OfType<XmlElement>()
                .Where(node => node.NodeType == XmlNodeType.Element && node.Name.Equals("analyzer"));
            provider = GetContainer(new ContainerDefinition(configNode.ParentNode as XmlElement));
            return analyzers;
        }


        private IContainer CreateSingleNodeContainer(XmlNode configNode, string elementName)
        {
            var document = new XmlDocument();
            if (configNode.OwnerDocument != null)
            {
                document.LoadXml("<root>" + configNode.OwnerDocument.SelectSingleNode("//" + elementName)?.OuterXml +
                                 "</root>");
            }

            return GetContainer(new ContainerDefinition(document.DocumentElement));
        }
    }
}