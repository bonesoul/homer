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
using Homer.Platform.HomeKit.Accessories.Info;
using Homer.Platform.HomeKit.Bridges;
using Homer.Platform.HomeKit.Caches;
using Homer.Platform.HomeKit.Caches.Identifiers;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Services;
using Homer.Platform.HomeKit.Services.Definitions;
using Serilog;
using uuid.net.Classes.UUID;
using uuid.net.Static_Classes.UUID_Validator;

namespace Homer.Platform.HomeKit.Accessories
{
    public class AccessoryBase : IAccessoryBase
    {
        public UUID Uuid { get; }

        public string DisplayName { get; }

        public int? AccessoryId { get; private set; }

        public bool IsBridged { get; }

        public bool IsReachable { get; }

        public AccessoryCategory Category { get; }

        public IReadOnlyDictionary<Type, IService> Services { get; }

        public IAccessoryInfo AccessoryInfo { get; private set; }

        public IIdentifierCache IdentifierCache { get; private set; }

        public ILogger Logger { get; protected set; }

        public ILogger AccessoryLogger { get; protected set; }

        /// <summary>
        /// internal list of services.
        /// </summary>
        private Dictionary<Type, IService> _services;

        protected AccessoryBase(string uuid, string displayName, bool isBridged, bool isReachable, AccessoryCategory category)
        {
            Uuid = uuid ?? throw new ArgumentException("Must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Must be created with a non-empty displayName.", nameof(displayName));

            Logger = Log.ForContext<AccessoryBase>();

            AccessoryId = null;
            IsBridged = isBridged;
            IsReachable = isReachable;
            Category = category;

            _services = new Dictionary<Type, IService>();
            Services = new ReadOnlyDictionary<Type, IService>(_services);

            // create our initial "Accessory Information" Service that all Accessories are expected to have.
            AddService(new AccessoryInformationService())
                .SetCharacteristic(typeof(ManufacturerCharacteristic), "Default-Manufacturer")
                .SetCharacteristic(typeof(ModelCharacteristic), "Default-Model")
                .SetCharacteristic(typeof(SerialNumberCharacteristic), "Default-SerialNumber")
                .SetCharacteristic(typeof(FirmwareRevisionCharacteristic), "0.1");

            AddService(new ProtocolInformationService())
                .SetCharacteristic(typeof(VersionCharacteristic), "1.1.0");
        }

        public IService AddService(IService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            _services.Add(service.GetType(), service);
            return service;
        }

        public IService GetService(Type service)
        {
            return _services.ContainsKey(service) ? _services[service] : null;
        }

        public void Publish(dynamic info, bool allowInsecureAccess = false)
        {
            AccessoryInfo = new AccessoryInfo(this, info);
            IdentifierCache = new IdentifierCache(info);
            AssignIds(IdentifierCache);

            LogAccessorySummary();
        }

        public void LogAccessorySummary()
        {
            Logger.Verbose("[{Type}] name: {Name}", this.GetType().Name, DisplayName);
            Logger.Verbose("-------------------------------------------------");
            Logger.Verbose("uuid: {Uuid}", Uuid);
            Logger.Verbose("accessory id: {Aid}", AccessoryId);

            foreach (var (key, service) in _services)
            {
                Logger.Verbose("service: [{Type}]", key.Name);

                foreach (var (type, characteristic) in service.Characteristics)
                {
                    Logger.Verbose("characteristic: [{Type}] => ({Format}) {Value}", type.Name, ((ICharacteristicProps)characteristic).Format, characteristic.Value);
                }

                foreach (var (type, characteristic) in service.OptionalCharacteristics)
                {
                    Logger.Verbose("optional characteristic: [{Type}] => ({Format}) {Value}", type.Name, ((ICharacteristicProps)characteristic).Format, characteristic.Value);
                }
            }
        }

        /// <summary>
        /// Assigns aid/iid to ourselves, any Accessories we are bridging, and all associated Services+Characteristics. Uses
        /// the provided identifierCache to keep IDs stable.
        /// </summary>
        /// <param name="identifierCache"></param>
        public void AssignIds(IIdentifierCache identifierCache)
        {
            if (!IsBridged && this is IBridge)
            {
                // as we are the bridge, we must have id = 1.
                AccessoryId = 1;
            }

            foreach (var (_, service) in Services)
            {
                if (this is IBridge)
                    service.AssignIds(IdentifierCache, this, 2000000000);
                else
                    service.AssignIds(identifierCache, this);
            }
        }
    }
}
