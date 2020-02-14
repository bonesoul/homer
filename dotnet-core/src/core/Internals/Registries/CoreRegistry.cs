using System;
using Homer.Core.Internals.Logging;
using Stashbox;

namespace Homer.Core.Internals.Registries
{
    /// <summary>
    /// Core object registry.
    /// </summary>
    public class CoreRegistry : IRegistry
    {
        // ioc container.
        private readonly StashboxContainer _container;

        public CoreRegistry(StashboxContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        public void Attach()
        {
            _container.RegisterSingleton<ILogManager, LogManager>();
        }
    }
}
