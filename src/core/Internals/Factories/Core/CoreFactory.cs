using System;
using Homer.Core.Internals.Logging;
using Stashbox;

namespace Homer.Core.Internals.Factories.Core
{
    /// <summary>
    /// Factory for core objects.
    /// </summary>
    public class CoreFactory : ICoreFactory
    {
        // ioc container.
        private readonly IDependencyResolver _resolver;

        /// <summary>
        /// Creates a new instance of core factory.
        /// </summary>
        /// <param name="resolver">IoC container</param>
        public CoreFactory(IDependencyResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <inheritdoc />
        public ILogManager GetLogManager()
        {
            return _resolver.Resolve<ILogManager>();
        }
    }
}
