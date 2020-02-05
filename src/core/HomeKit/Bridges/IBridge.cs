using System.Collections.Generic;
using Smarty.Core.HomeKit.Accessories;

namespace Smarty.Core.HomeKit.Bridges
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
