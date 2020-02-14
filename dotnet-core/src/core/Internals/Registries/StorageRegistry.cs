using System;
using Stashbox;

namespace Homer.Core.Internals.Registries
{
    /// <summary>
    /// Storage registry.
    /// </summary>
    public class StorageRegistry : IRegistry
    {
        // ioc container.
        private readonly StashboxContainer _container;

        public StorageRegistry(StashboxContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <inheritdoc />
        public void Attach()
        {
        }
    }
}
