namespace Cerberus.Core.Configuration
{
    using System;
    using System.Collections.Generic;

    public class ReadOnlyConfiguration : IConfiguration
    {
        private readonly IConfiguration _innerConfiguration;

        public ReadOnlyConfiguration(IConfiguration innerConfiguration)
        {
            _innerConfiguration =
                innerConfiguration ?? throw new InvalidOperationException("innerConfiguration is null.");
        }

        public string Name => _innerConfiguration.Name;
        public string Description => _innerConfiguration.Description;
        public string Extends => _innerConfiguration.Extends;
        public string[] Dependencies => _innerConfiguration.Dependencies;
        public string[] IgnoredImplicitDependencies => _innerConfiguration.IgnoredImplicitDependencies;

        public T Resolve<T>() where T : class
        {
            return _innerConfiguration.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _innerConfiguration.Resolve(type);
        }

        public void Register(Type type, Func<object> factory, bool singleInstance)
        {
            throw new InvalidOperationException("You cannot register new dependencies on a read-only configuration.");
        }

        public object Activate(Type type, KeyValuePair<string, object>[] unmappedConstructorParameters)
        {
            return _innerConfiguration.Activate(type, unmappedConstructorParameters);
        }

        public void Assert(Type type)
        {
            _innerConfiguration.Assert(type);
        }

        public void AssertSingleton(Type type)
        {
            _innerConfiguration.AssertSingleton(type);
        }

        public void AssertTransient(Type type)
        {
            _innerConfiguration.AssertTransient(type);
        }
    }
}