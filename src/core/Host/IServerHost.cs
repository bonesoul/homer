using System.Collections.Generic;
using System.Threading.Tasks;
using Homer.Core.Internals.Registries;

namespace Homer.Core.Host
{
    public interface IServerHost
    {
        /// <summary>
        /// Initializes service host.
        /// </summary>
        /// <param name="registries"></param>
        /// <param name="args"></param>
        /// <param name="needsNetworkingSupport">does host need networking support?</param>
        Task InitializeAsync(IReadOnlyList<IRegistry> registries, string[] args, bool needsNetworkingSupport = false);
    }
}
