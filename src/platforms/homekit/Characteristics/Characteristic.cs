#region license
// 
//     homer - The complete home automation for Homer Simpson.
//     Copyright (C) 2020, Hüseyin Uslu - shalafiraistlin at gmail dot com
//     https://github.com/bonesoul/homer
// 
//      “Commons Clause” License Condition v1.0
//
//      The Software is provided to you by the Licensor under the License, as defined below, subject to the following condition.
//  
//      Without limiting other conditions in the License, the grant of rights under the License will not include, and the License
//      does not grant to you, the right to Sell the Software.
//
//      For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide
//      to third parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support
//      services related to the Software), a product or service whose value derives, entirely or substantially, from the functionality
//      of the Software.Any license notice or attribution required by the License must also include this Commons Clause License
//      Condition notice.
//
//      License: MIT License
//      Licensor: Hüseyin Uslu
#endregion

using System;
using System.Collections.Generic;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Events;

namespace Homer.Platform.HomeKit.Characteristics
{
    public class Characteristic : EventEmitter, ICharacteristic, ICharacteristicProps
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public int InstanceId { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        public dynamic Value { get; private set; }

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

            Value = GetDefaultValue();
        }

        public void SetValue(dynamic value)
        {
            Value = value;
        }

        private dynamic GetDefaultValue()
        {
            return Format switch
            {
                CharacteristicFormat.Bool => (dynamic) false,
                CharacteristicFormat.Int => 0,
                CharacteristicFormat.Float => 0,
                CharacteristicFormat.String => "",
                CharacteristicFormat.Uint8 => 0,
                CharacteristicFormat.Uint16 => 0,
                CharacteristicFormat.Uint32 => 0,
                CharacteristicFormat.Uint64 => 0,
                CharacteristicFormat.Data => null,
                CharacteristicFormat.Tlv8 => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
