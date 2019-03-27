namespace Cerberus.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using Configy.Containers;

    public static class XmlActivator
    {
        public static T CreateObject<T>(XmlNode node)
            where T : class
        {
            var typeString = node.Attributes?["type"]?.InnerText;
            if (typeString == null)
            {
                return null;
            }

            var type = Type.GetType(typeString);

            if (type == null)
            {
                return null;
            }

            var obj = Activator.CreateInstance(type) as T;

            var properties = typeof(T).GetProperties().ToList();
            foreach (XmlAttribute attr in node.Attributes)
            {
                var prop = properties.FirstOrDefault(p =>
                    string.Equals(p.Name, attr.Name, StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    var val = attr.Value ?? string.Empty;
                    prop.SetValue(obj, val);
                }
            }

            return obj;
        }

        public static object CreateInstance(Type type, XmlElement node, IContainer[] providers)
        {
            var constructorInfo = type.GetConstructors().FirstOrDefault();
            var prams = new List<object>();
            if (constructorInfo != null)
            {
                var parameterInfos = constructorInfo.GetParameters().OrderBy(p => p.Position).ToList();
                foreach (var parameterInfo in parameterInfos)
                {
                    if (parameterInfo.ParameterType.IsPrimitive || parameterInfo.ParameterType == typeof(string))
                    {
                        AddPrimitiveParam(node, parameterInfo, prams);
                    }
                    else if (parameterInfo.ParameterType.IsEnum)
                    {
                        var enumType = Assembly.GetExecutingAssembly().DefinedTypes.FirstOrDefault(p =>
                            p.Name.Equals(parameterInfo.Name, StringComparison.InvariantCultureIgnoreCase) && p.IsEnum);
                        if (enumType != null)
                        {
                            var p = node.Attributes[parameterInfo.Name];
                            var enumValue = Enum.Parse(enumType,
                                p?.Value ?? throw new InvalidOperationException("Error Action is not configured."),
                                true);
                            prams.Add(enumValue);
                        }
                    }
                    else
                    {
                        AddRefObjectParam(providers, parameterInfo, prams);
                    }
                }
            }

            return Activator.CreateInstance(type, prams.ToArray());
        }

        private static void AddRefObjectParam(IContainer[] providers, ParameterInfo parameterInfo, List<object> prams)
        {
            object item = null;
            foreach (var container in providers)
            {
                item = container.Resolve(parameterInfo.ParameterType);
                if (item != null)
                {
                    break;
                }
            }

            prams.Add(item);
        }

        private static void AddPrimitiveParam(XmlElement node, ParameterInfo parameterInfo, List<object> prams)
        {
            var p = node.Attributes[parameterInfo.Name];
            if (p != null && !string.IsNullOrEmpty(p.Value))
            {
                prams.Add(Convert.ChangeType(p.Value, parameterInfo.ParameterType));
            }
        }

        public static Type GetType(XmlElement node)
        {
            var typeString = node.Attributes["type"]?.InnerText;
            if (typeString != null)
            {
                return Type.GetType(typeString);
            }

            return null;
        }
    }
}