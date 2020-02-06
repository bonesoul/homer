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
using DaanV2.UUID;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Characteristics.Definitions;

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
