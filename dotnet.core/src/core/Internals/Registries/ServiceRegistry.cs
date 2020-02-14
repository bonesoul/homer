using System;
using Homer.Core.Internals.Services.Configuration;
using Homer.Core.Internals.Services.Runtime;
using Stashbox;

namespace Homer.Core.Internals.Registries
{
    /// <summary>
    /// Service registry.
    /// </summary>
    public class ServiceRegistry : IRegistry
    {
        // ioc container.
        private readonly StashboxContainer _container;

        public ServiceRegistry(StashboxContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        public void Attach()
        {
            _container.RegisterSingleton<IRuntimeInfoService, RuntimeInfoService>();
            _container.RegisterSingleton<IConfigurationService, ConfigurationService>();
        }
    }
}
