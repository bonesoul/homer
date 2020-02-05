using System.Collections.Generic;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Services;

namespace Homer.Platform.HomeKit.Accessories
{
    /// <summary>
    /// Base accessory interface.
    /// </summary>
    public interface IAccessoryBase
    {
        /// <summary>
        /// UUID of the accessory.
        /// </summary>
        public UUID Uuid { get; }

        /// <summary>
        /// Display name of the accessory.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// true if we are hosted "behind" a Bridge Accessory
        /// </summary>
        public bool IsBridged { get; }

        /// <summary>
        /// Is accessory reachable?
        /// </summary>
        public bool IsReacheable { get; }

        /// <summary>
        /// Accessory category.
        /// </summary>
        public AccessoryCategory Category { get; }

        /// <summary>
        /// Services exposed by accessory.
        /// </summary>
        public IReadOnlyList<IService> Services { get; }
    }
}
