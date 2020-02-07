using System;
using Homer.Core.Internals.Registries;
using Homer.Servers;
using Stashbox;

namespace Homer.Internals
{
    public class ServerRegistry : IRegistry
    {
        // ioc container.
        private readonly StashboxContainer _container;

        public ServerRegistry(StashboxContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Attach()
        {
            _container.RegisterSingleton<IServer, Server>();
        }
    }
}
