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
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Entity;

namespace Homer.Platform.HomeKit.Services
{
    /// <summary>
    /// Services group functionality in order to provide context. In the aforementioned example accessory there are three
    /// services: a fan service to interact with the ceiling fan, a light service to interact with the light, and a mister service to
    /// interact with the spray mister.
    /// </summary>
    public interface IService: IEntity
    {
        /// <summary>
        /// instance IDs are numbers with a range of [1, 18446744073709551615] for IP accessories (see ”7.4.4.2 Instance IDs”
        /// (page 122) for BLE accessories). These numbers are used to uniquely identify HAP accessory objects within an HAP
        /// accessory server, or uniquely identify services, and characteristics within an HAP accessory object. The instance ID
        /// for each object must be unique for the lifetime of the server/client pairing.
        /// </summary>
        public int InstanceId { get; }

        /// <summary>
        /// Characteristics provided by the service.
        /// </summary>
        public IReadOnlyDictionary<Type, ICharacteristic> Characteristics { get; }

        /// <summary>
        /// Optional Characteristics provided by the service.
        /// </summary>
        public IReadOnlyDictionary<Type, ICharacteristic> OptionalCharacteristics { get; }

        /// <summary>
        /// Returns characteristic for given type.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public ICharacteristic GetCharacteristic(Type characteristic);

        /// <summary>
        /// Returns optional characteristic for given type.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public ICharacteristic GetOptionalCharacteristic(Type characteristic);

        /// <summary>
        /// Sets characteristic for given type.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IService SetCharacteristic(Type t, dynamic value);

        /// <summary>
        /// Sets optional characteristic for given type.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IService SetOptionalCharacteristic(Type t, dynamic value);
    }
}
