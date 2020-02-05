using System;
using DaanV2.UUID;

namespace Smarty.Platform.HomeKit.Services
{
    public class Service : IService
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        public Service(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Service must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Services must be created with a non-empty displayName.", nameof(displayName));
        }
    }
}
