namespace Sitecore.Helix.Validator.Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Configuration;
    using Configy;
    using Configy.Parsing;

    public sealed class LogDispatcher : XmlContainerBuilder, ILogDispatcher
    {
        private readonly IList<ILogger> _loggers;

        public LogDispatcher(XmlNode configNode, IDataSourceLocation dataSourceLocation) : base(
            new ContainerDefinitionVariablesReplacer())
        {
            var loggerConfigs = configNode.ChildNodes.OfType<XmlElement>().Where(node =>
                node.NodeType == XmlNodeType.Element && node.Name.Equals("logger"));
            var provider = GetContainer(new ContainerDefinition(configNode as XmlElement));
            provider.Register(typeof(IDataSourceLocation), () => dataSourceLocation, true);

            _loggers = new List<ILogger>();
            foreach (var config in loggerConfigs)
            {
                var type = Type.GetType(config.Attributes["type"]?.Value ??
                                        throw new InvalidOperationException("Invalid Logger Configuration."));
                if (type != null)
                {
                    var allInterfaces = type.GetInterfaces();
                    var directInterface = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));
                    _loggers.Add(provider.Resolve(directInterface.First()) as ILogger);
                }
            }
        }

        public void Log(string message, LogLevel log)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message, log);
            }
        }

        public void Info(string message)
        {
            Log(message, LogLevel.Information);
        }

        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Report()
        {
            foreach (var logger in _loggers)
            {
                logger.Report();
            }
        }

        public void InitLogger(string name, string description)
        {
            foreach (var logger in _loggers)
            {
                logger.InitLogger(name, description);
            }
        }
    }
}