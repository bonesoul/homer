using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Entity;

namespace Homer.Platform.HomeKit.Services
{
    public class Service : IService
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        public IReadOnlyDictionary<Type, ICharacteristic> Characteristics { get; }

        /// <summary>
        /// internal list of characteristics.
        /// </summary>
        private readonly Dictionary<Type, ICharacteristic> _characteristics;

        public Service(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Service must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Services must be created with a non-empty displayName.", nameof(displayName));

            _characteristics = new Dictionary<Type, ICharacteristic>();
            Characteristics = new ReadOnlyDictionary<Type, ICharacteristic>(_characteristics);

            SetCharacteristic(typeof(NameCharacteristic), displayName); // set Characteristic.Name to given displayName.
        }

        public IService SetCharacteristic(Type t, dynamic value)
        {
            var characteristic = GetCharacteristic(t) ?? AddCharacteristic((ICharacteristic)Activator.CreateInstance(t));
            characteristic.SetValue(value);
            return this; // allow chaining.
        }

        private ICharacteristic GetCharacteristic(Type characteristic)
        {
            return _characteristics.ContainsKey(characteristic) ? _characteristics[characteristic] : null;
        }

        private ICharacteristic AddCharacteristic(ICharacteristic characteristic)
        {
            _characteristics.Add(characteristic.GetType(), characteristic);
            return characteristic;
        }
    }
}
