using System.Collections.Generic;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Entity;
using Homer.Platform.HomeKit.Services;

namespace Homer.Platform.HomeKit.Accessories
{
    /// <summary>
    /// Base accessory interface.
    /// </summary>
    public interface IAccessoryBase : IEntity
    {
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
