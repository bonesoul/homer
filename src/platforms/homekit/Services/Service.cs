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
using System.Collections.ObjectModel;
using System.Linq;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Events;
using uuid.net.Classes.UUID;
using uuid.net.Static_Classes.UUID_Validator;

namespace Homer.Platform.HomeKit.Services
{
    public class Service : EventEmitter, IService
    {
        /// <inheritdoc />
        public UUID Uuid { get; }

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        public int InstanceId { get; }

        /// <inheritdoc />
        public IReadOnlyDictionary<Type, ICharacteristic> Characteristics { get; }

        /// <inheritdoc />
        public IReadOnlyDictionary<Type, ICharacteristic> OptionalCharacteristics { get; }

        /// <inheritdoc />
        public bool IsPrimaryService { get; }

        /// <inheritdoc />
        public bool IsHiddenService
        {
            get => _isHiddenService;
            set
            {
                _isHiddenService = value;
                OnEvent(ServiceConfigurationChange, EventArgs.Empty); // notify listeners.
            }
        }

        /// <inheritdoc />
        public List<IService> LinkedServices { get; }

        /// <inheritdoc />
        public event EventHandler<EventArgs> CharacteristicChange;

        /// <inheritdoc />
        public event EventHandler<EventArgs> ServiceConfigurationChange;

        // internal list of characteristics.
        private readonly Dictionary<Type, ICharacteristic> _characteristics;
        private readonly Dictionary<Type, ICharacteristic> _optionalCharacteristics;

        // internally track if this is a hidden service.
        private bool _isHiddenService;

        public Service(string uuid, string displayName)
        {
            Uuid = uuid ?? throw new ArgumentException("Service must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Services must be created with a non-empty displayName.", nameof(displayName));

            // set characteristics lists.
            _characteristics = new Dictionary<Type, ICharacteristic>();
            _optionalCharacteristics = new Dictionary<Type, ICharacteristic>();
            Characteristics = new ReadOnlyDictionary<Type, ICharacteristic>(_characteristics);
            OptionalCharacteristics = new ReadOnlyDictionary<Type, ICharacteristic>(_optionalCharacteristics);

            // set Characteristic.Name to given displayName.
            SetCharacteristic(typeof(NameCharacteristic), displayName);

            IsPrimaryService = false;
            IsHiddenService = false;
            LinkedServices = new List<IService>();
        }

        public ICharacteristic GetCharacteristic(Type characteristic)
        {
            return _characteristics.ContainsKey(characteristic) ? _characteristics[characteristic] : null;
        }

        public ICharacteristic GetOptionalCharacteristic(Type characteristic)
        {
            return _optionalCharacteristics.ContainsKey(characteristic) ? _optionalCharacteristics[characteristic] : null;
        }

        public IService AddCharacteristic(ICharacteristic characteristic)
        {
            _characteristics.Add(characteristic.GetType(), characteristic);
            OnEvent(ServiceConfigurationChange, EventArgs.Empty); // notify listeners.
            return this; // allow chaining.
        }

        public IService AddCharacteristic(Type t)
        {
            var characteristic = (ICharacteristic)Activator.CreateInstance(t);
            AddCharacteristic(characteristic);
            return this; // allow chaining.
        }

        public IService AddOptionalCharacteristic(ICharacteristic characteristic)
        {
            _optionalCharacteristics.Add(characteristic.GetType(), characteristic);
            OnEvent(ServiceConfigurationChange, EventArgs.Empty); // notify listeners.
            return this; // allow chaining.
        }

        public IService AddOptionalCharacteristic(Type t)
        {
            var optionalCharacteristic = (ICharacteristic)Activator.CreateInstance(t);
            AddOptionalCharacteristic(optionalCharacteristic);
            return this; // allow chaining.
        }

        public IService SetCharacteristic(Type t, dynamic value)
        {
            var characteristic = GetCharacteristic(t);

            if (characteristic == null)
            {
                AddCharacteristic(t);
                characteristic = GetCharacteristic(t);
            }

            characteristic.SetValue(value);
            return this; // allow chaining.
        }

        public IService SetOptionalCharacteristic(Type t, dynamic value)
        {
            var optionalCharacteristic = GetOptionalCharacteristic(t);

            if (optionalCharacteristic == null)
            {
                AddOptionalCharacteristic(t);
                optionalCharacteristic = GetOptionalCharacteristic(t);
            }

            optionalCharacteristic.SetValue(value);
            return this; // allow chaining.
        }

        public IService RemoveCharacteristic(Type t)
        {
            if (_characteristics.Any(x => x.Value.GetType() == t))
            {
                _characteristics.Remove(t);
                // TODO: remove listeners of characteristic too.
                OnEvent(ServiceConfigurationChange, EventArgs.Empty); // notify listeners.
            }

            return this; // allow chaining.
        }

        public IService RemoveOptionalCharacteristic(Type t)
        {
            if (_optionalCharacteristics.Any(x => x.Value.GetType() == t))
            {
                _optionalCharacteristics.Remove(t);
                // TODO: remove listeners of characteristic too.
                OnEvent(ServiceConfigurationChange, EventArgs.Empty); // notify listeners.
            }

            return this; // allow chaining.
        }

        public bool CheckCharacteristic(Type t)
        {
            return _characteristics.ContainsKey(t);
        }

        public bool CheckOptionalCharacteristic(Type t)
        {
            return _optionalCharacteristics.ContainsKey(t);
        }

        public ICharacteristic GetCharacteristicByInstanceId(int iid)
        {
            return (from x in _characteristics where x.Value.InstanceId == iid select x.Value).FirstOrDefault();
        }

        public ICharacteristic GetOptionalCharacteristicByInstanceId(int iid)
        {
            return (from x in _optionalCharacteristics where x.Value.InstanceId == iid select x.Value).FirstOrDefault();
        }

        public string ToHapJson()
        {
            return "";
        }
    }
}
