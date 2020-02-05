using Homer.Platform.HomeKit.Bridges;
using Homer.Platform.HomeKit.Entity;

namespace Homer.Platform.HomeKit.Accessories
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
