using System.Collections.Generic;
using Homer.Core.Internals.Registries;
using Stashbox;

namespace Homer.Core.Internals.Bootstrap
{
    /// <summary>
    /// IoC container kernel bootstrapper
    /// </summary>
    public class Bootstrapper : IBootstrapper
    {
        /// <inheritdoc />
        public StashboxContainer Container { get; }

        // list of registries.
        private readonly List<IRegistry> _registries;

        /// <summary>
        /// Creates a new instance of bootstrapper.
        /// </summary>
        public Bootstrapper()
        {
            Container = new StashboxContainer(); // create the container.

            // create the list of registries.
            _registries = new List<IRegistry>
            {
                new CoreRegistry(Container),
                new FactoryRegistry(Container),
                new ServiceRegistry(Container),
                new StorageRegistry(Container),
            };
        }

        /// <summary>
        /// Runs and handles the registries attached.
        /// </summary>
        /// <param name="additional"></param>
        public void Run(IList<IRegistry> additional)
        {
            _registries.AddRange(additional); // add additional registries if any.
            AttachRegistries(); // handle main registries.
        }

        /// <summary>
        /// Attaches the registries.
        /// </summary>
        private void AttachRegistries()
        {
            foreach (var registry in _registries) // loop through registries.
            {
                registry.Attach(); // attach the registry.
            }
        }
    }
}
