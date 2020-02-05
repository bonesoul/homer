using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DaanV2.UUID;
using Smarty.Core.HomeKit.Accessory;
using Smarty.Core.HomeKit.Service;

namespace Smarty.Core.HomeKit.Bridge
{
    public class Bridge : IBridge
    {
        const int MaxAccessories = 149; // Maximum number of bridged accessories per bridge.

        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        public bool IsBridged { get; }

        /// <inheritdoc />
        public bool IsReacheable { get; }

        /// <inheritdoc />
        public AccessoryCategory Category { get; }

        /// <inheritdoc />
        public IReadOnlyList<IAccessoryBase> Accessories { get; }

        /// <inheritdoc />
        public IReadOnlyList<IService> Services { get; }

        /// <summary>
        /// internal list of accessories.
        /// </summary>
        private List<IAccessoryBase> _accessories;

        /// <summary>
        /// internal list of services.
        /// </summary>
        private List<IService> _services;

        public Bridge(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Accessories must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Accessories must be created with a non-empty displayName.", nameof(displayName));

            IsBridged = false; // a bridge can not be bridged again.
            Category = AccessoryCategory.Bridge; // set category.

            _accessories = new List<IAccessoryBase>();
            _services = new List<IService>();

            Accessories = new ReadOnlyCollection<IAccessoryBase>(_accessories);
            Services = new ReadOnlyCollection<IService>(_services);
        }
    }
}
