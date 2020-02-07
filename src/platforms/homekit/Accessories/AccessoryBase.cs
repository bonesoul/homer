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
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Services;
using Homer.Platform.HomeKit.Services.Definitions;
using uuid.net.Classes.UUID;
using uuid.net.Static_Classes.UUID_Validator;

namespace Homer.Platform.HomeKit.Accessories
{
    public class AccessoryBase : IAccessoryBase
    {
        public UUID Uuid { get; }

        public string DisplayName { get; }

        public bool IsBridged { get; }

        public bool IsReachable { get; }

        public AccessoryCategory Category { get; }

        public IReadOnlyList<IService> Services { get; }

        /// <summary>
        /// internal list of services.
        /// </summary>
        private List<IService> _services;

        protected AccessoryBase(string uuid, string displayName, bool isBridged, bool isReachable, AccessoryCategory category)
        {
            Uuid = uuid ?? throw new ArgumentException("Must be created with a valid UUID.", nameof(uuid));
            if (!UUIDValidator.IsValidUUID(Uuid)) throw new ArgumentException("Provided UUID is not valid.", nameof(uuid));

            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            else throw new ArgumentException("Must be created with a non-empty displayName.", nameof(displayName));

            IsBridged = isBridged;
            IsReachable = isReachable;
            Category = category;

            _services = new List<IService>();
            Services = new ReadOnlyCollection<IService>(_services);

            // create our initial "Accessory Information" Service that all Accessories are expected to have.
            AddService(new AccessoryInformationService())
                .SetCharacteristic(typeof(ManufacturerCharacteristic), "Hüseyin Uslu")
                .SetCharacteristic(typeof(ModelCharacteristic), "Homer")
                .SetCharacteristic(typeof(SerialNumberCharacteristic), "CC:22:3D:E3:CE:30")
                .SetCharacteristic(typeof(FirmwareRevisonCharacteristic), "0.1");
        }

        public IService AddService(IService service)
        {
            _services.Add(service);
            return service; // allow chaining.
        }
    }
}
