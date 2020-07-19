using System;
using Homer.Core.Internals.Factories.Core;
using Stashbox;

namespace Homer.Core.Internals.Registries
{
    /// <summary>
    /// Factory registry.
    /// </summary>
    public class FactoryRegistry : IRegistry
    {
        // ioc container.
        private readonly StashboxContainer _container;

        public FactoryRegistry(StashboxContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        public void Attach()
        {
            _container.RegisterSingleton<ICoreFactory, CoreFactory>();
        }
    }
}
