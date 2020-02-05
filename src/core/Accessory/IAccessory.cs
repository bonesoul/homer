using System;
using System.Collections.Generic;
using System.Text;
using Smarty.Core.Bridge;

namespace Smarty.Core.Accessory
{
    /// <summary>
    /// Accessory interface.
    /// </summary>
    public interface IAccessory : IAccessoryBase
    {
        /// <summary>
        /// if accessory is bridged, this property points to the bridge which bridges this accessory
        /// </summary>
        public IBridge ParentBridge { get; }
    }
}
