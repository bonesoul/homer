using System.Collections.Generic;
using Smarty.Platform.HomeKit.Accessories;

namespace Smarty.Platform.HomeKit.Bridges
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
