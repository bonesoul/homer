using System.Collections.Generic;
using Homer.Platform.HomeKit.Accessories;

namespace Homer.Platform.HomeKit.Bridges
{
    /// <summary>
    /// Bridge interface.
    /// </summary>
    public interface IBridge : IAccessoryBase
    {
        /// <summary>
        /// List of accessories we are bridging.
        /// </summary>
        IReadOnlyList<IAccessoryBase> Accessories { get; }
    }
}
