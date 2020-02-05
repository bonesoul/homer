using System;
using DaanV2.UUID;
using Smarty.Core.HomeKit.Events;

namespace Smarty.Core.HomeKit.Characteristics
{
    public class Characteristic : EventEmitter, ICharacteristic
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        public Characteristic(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Characteristics must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Characteristics must be created with a non-empty displayName.", nameof(displayName));
        }
    }
}
