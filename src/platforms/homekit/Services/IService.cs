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
        int InstanceId { get; }

        /// <summary>
        /// Characteristics provided by the service.
        /// </summary>
        IReadOnlyDictionary<Type, ICharacteristic> Characteristics { get; }

        /// <summary>
        /// Optional Characteristics provided by the service.
        /// <remarks>
        /// To maintain backward compatibility with earlier clients, any characteristics added in later versions of a service must
        /// be optional.Later versions of a service must not change behaviors defined in previous versions of the service.
        /// </remarks>
        /// </summary>
        IReadOnlyDictionary<Type, ICharacteristic> OptionalCharacteristics { get; }

        /// <summary>
        /// Accessories should list one of its services as the primary service. The primary service must match the primary function
        /// of the accessory and must also match with the accessory category.An accessory must expose only one primary
        /// service from its list of available services.
        /// </summary>
        bool IsPrimaryService { get; }

        /// <summary>
        /// Accessories may specify the services that are to be hidden from users by a generic HomeKit application. Accessories
        /// may expose several services that could be used to configure the accessory or to update firmware on the accessory,
        /// these services should be marked as hidden.When all characteristics in a service are marked hidden then the service
        /// must also be marked as hidden
        /// </summary>
        bool IsHiddenService { get; }

        /// <summary>
        /// Linked services allows accessories to specify logical relationship between services. A service can link to one or more
        /// services.A service must not link to itself.Service links have context and meaning only to the first level of services that
        /// it links to.For example if Service A links to Service B, and service B links to Service C, this does not imply any relation
        /// between Service A to Service C.If Service A also relates to Service C then Service Aʼs linked services must include
        /// both Service B and Service C.Linked services allows applications to display logically grouped accessory controls in
        /// the UI.
        /// </summary>
        List<IService> LinkedServices { get; }

        /// <summary>
        /// Event for characteristics changes.
        /// </summary>
        event EventHandler<EventArgs> CharacteristicChange;

        /// <summary>
        /// Event for service configuration changes.
        /// </summary>
        event EventHandler<EventArgs> ServiceConfigurationChange;

        /// <summary>
        /// Returns characteristic for given type.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        ICharacteristic GetCharacteristic(Type characteristic);

        /// <summary>
        /// Returns optional characteristic for given type.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        ICharacteristic GetOptionalCharacteristic(Type characteristic);

        IService AddCharacteristic(ICharacteristic characteristic);

        IService AddCharacteristic(Type t);

        IService AddOptionalCharacteristic(ICharacteristic characteristic);

        IService AddOptionalCharacteristic(Type t);

        /// <summary>
        /// Sets characteristic for given type.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IService SetCharacteristic(Type t, dynamic value);

        /// <summary>
        /// Sets optional characteristic for given type.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IService SetOptionalCharacteristic(Type t, dynamic value);

        IService RemoveCharacteristic(Type t);

        IService RemoveOptionalCharacteristic(Type t);

        bool CheckCharacteristic(Type t);

        bool CheckOptionalCharacteristic(Type t);

        ICharacteristic GetCharacteristicByInstanceId(int iid);

        ICharacteristic GetOptionalCharacteristicByInstanceId(int iid);

        IService ToHapJson();
    }
}
