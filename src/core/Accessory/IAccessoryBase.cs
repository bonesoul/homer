using System;
using System.Collections.Generic;
using System.Text;
using DaanV2.UUID;
using Smarty.Core.Bridge;

namespace Smarty.Core.Accessory
{
    /// <summary>
    /// Base accessory interface.
    /// </summary>
    public interface IAccessoryBase
    {
        /// <summary>
        /// UUID of the accessory.
        /// </summary>
        public UUID Id { get; }

        /// <summary>
        /// true if we are hosted "behind" a Bridge Accessory
        /// </summary>
        public bool IsBridged { get; }

        /// <summary>
        /// Is accessory reachable?
        /// </summary>
        public bool IsReacheable { get; }
    }
}
