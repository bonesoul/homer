using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DaanV2.UUID;
using Smarty.Core.HomeKit.Bridges;
using Smarty.Core.HomeKit.Services;

namespace Smarty.Core.HomeKit.Accessories
{
    public class Accessory : IAccessory
    {
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
        public IReadOnlyList<IService> Services { get; }

        /// <inheritdoc />
        public IBridge ParentBridge { get; }

        /// <summary>
        /// internal list of services.
        /// </summary>
        private List<IService> _services;

        public Accessory(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Accessories must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Accessories must be created with a non-empty displayName.", nameof(displayName));

            IsBridged = false;
            IsReacheable = true;
            Category = AccessoryCategory.Other;

            _services = new List<IService>();
            Services = new ReadOnlyCollection<IService>(_services);

            // create our initial "Accessory Information" Service that all Accessories are expected to have.
        }
    }
}
