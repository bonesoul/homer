using System;
using System.Collections.Generic;
using DaanV2.UUID;
using Smarty.Platform.HomeKit.Events;

namespace Smarty.Platform.HomeKit.Characteristics
{
    public class Characteristic : EventEmitter, ICharacteristic, ICharacteristicProps
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        public dynamic Value { get; }

        /// <inheritdoc />
        public IReadOnlyList<CharacteristicPermission> Permissions { get; }

        /// <inheritdoc />
        public bool EventNotificationsEnabled { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public CharacteristicFormat Format { get; }

        /// <inheritdoc />
        public CharacteristicUnit Unit { get; }

        /// <inheritdoc />
        public dynamic MinValue { get; }

        /// <inheritdoc />
        public dynamic MaxValue { get; }

        /// <inheritdoc />
        public dynamic MinStep { get; }

        /// <inheritdoc />
        public int MaxLength { get; }

        /// <inheritdoc />
        public int MaxDataLength { get; }

        /// <inheritdoc />
        public IList<dynamic> ValidValues { get; }

        /// <inheritdoc />
        public dynamic[] ValidValuesRange { get; }


        protected Characteristic(string uuid, string displayName, CharacteristicFormat format,  IReadOnlyList<CharacteristicPermission> permissions,  
            CharacteristicUnit unit = CharacteristicUnit.Unitless,  bool eventNotificationsEnabled = false,  string description = null, 
            dynamic minValue = null,  dynamic maxValue = null, dynamic minStep = null,  int maxLength = 64, int maxDataLength = 2097152, 
            IList<dynamic> validValues = null, dynamic[] validValuesRange = null)
        {
            Uuid = uuid ?? throw new ArgumentException("Characteristics must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Characteristics must be created with a non-empty displayName.", nameof(displayName));

            Value = GetDefaultValue();
            
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            if (Permissions.Count == 0) throw new ArgumentException("Characteristics must have permissions defined.", nameof(displayName));

            Format = format;
            Unit = unit;

            EventNotificationsEnabled = eventNotificationsEnabled;
            Description = description;
            MinValue = minValue;
            MaxValue = maxValue;
            MinStep = minStep;
            MaxLength = maxLength;
            MaxDataLength = maxDataLength;
            ValidValues = validValues;
            ValidValuesRange = validValuesRange;
        }

        private dynamic GetDefaultValue()
        {
            switch (Format)
            {
                case CharacteristicFormat.Bool:
                    return false;
                case CharacteristicFormat.Int:
                    return 0;
                case CharacteristicFormat.Float:
                    return 0;
                case CharacteristicFormat.String:
                    return "";
                case CharacteristicFormat.Uint8:
                    return 0;
                case CharacteristicFormat.Uint16:
                    return 0;
                case CharacteristicFormat.Uint32:
                    return 0;
                case CharacteristicFormat.Uint64:
                    return 0;
                case CharacteristicFormat.Data:
                    return null;
                case CharacteristicFormat.Tlv8:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
