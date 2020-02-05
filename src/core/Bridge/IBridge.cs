using System;
using System.Collections.Generic;
using System.Text;
using Smarty.Core.Accessory;

namespace Smarty.Core.Bridge
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
