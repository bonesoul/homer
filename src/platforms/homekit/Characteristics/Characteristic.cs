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
        public int Subscriptions { get; }

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

        /// <inheritdoc />
        public event EventHandler<EventArgs> Get;

        /// <inheritdoc />
        public event EventHandler<EventArgs> Set;

        /// <inheritdoc />
        public event EventHandler<EventArgs> OnSubscribe;

        /// <inheritdoc />
        public event EventHandler<EventArgs> OnUnsubscribe;

        /// <inheritdoc />
        public event EventHandler<EventArgs> Change;

        /**
        * Characteristic represents a particular typed variable that can be assigned to a Service. For instance, a
        * "Hue" Characteristic might store a 'float' value of type 'arcdegrees'. You could add the Hue Characteristic
        * to a Service in order to store that value. A particular Characteristic is distinguished from others by its
        * UUID. HomeKit provides a set of known Characteristic UUIDs defined in HomeKit.ts along with a
        * corresponding concrete subclass.
        *
        * You can also define custom Characteristics by providing your own UUID. Custom Characteristics can be added
        * to any native or custom Services, but Siri will likely not be able to work with these.
        *
        * Note that you can get the "value" of a Characteristic by accessing the "value" property directly, but this
        * is really a "cached value". If you want to fetch the latest value, which may involve doing some work, then
        * call getValue().
        *
        * @event 'get' => function(callback(err, newValue), context) { }
        *        Emitted when someone calls getValue() on this Characteristic and desires the latest non-cached
        *        value. If there are any listeners to this event, one of them MUST call the callback in order
        *        for the value to ever be delivered. The `context` object is whatever was passed in by the initiator
        *        of this event (for instance whomever called `getValue`).
        *
        * @event 'set' => function(newValue, callback(err), context) { }
        *        Emitted when someone calls setValue() on this Characteristic with a desired new value. If there
        *        are any listeners to this event, one of them MUST call the callback in order for this.value to
        *        actually be set. The `context` object is whatever was passed in by the initiator of this change
        *        (for instance, whomever called `setValue`).
        *
        * @event 'change' => function({ oldValue, newValue, context }) { }
        *        Emitted after a change in our value has occurred. The new value will also be immediately accessible
        *        in this.value. The event object contains the new value as well as the context object originally
        *        passed in by the initiator of this change (if known).
        */

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

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public void GetValue()
        {
            throw new NotImplementedException();
        }

        public void SetValue(dynamic value)
        {
            Value = value;
        }

        public string ToHapJson()
        {
            throw new NotImplementedException();
        }

        private void ValidateValue(dynamic value)
        {

        }

        private void AssignInstanceId()
        {

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
