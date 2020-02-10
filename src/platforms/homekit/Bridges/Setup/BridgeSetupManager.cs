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
using Homer.Platform.HomeKit.Bridges.Setup.Characteristics;
using Homer.Platform.HomeKit.Bridges.Setup.Setup;
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Services.Definitions;
using VersionCharacteristic = Homer.Platform.HomeKit.Bridges.Setup.Characteristics.VersionCharacteristic;

namespace Homer.Platform.HomeKit.Bridges.Setup
{
    public class BridgeSetupManager : Bridge
    {
        public BridgeSetupManager(string uuid, string displayName) 
            : base(uuid, displayName)
        {
            // create handler characteristic.
            var controlPointCharacteristic = new ControlPointCharacteristic();
            controlPointCharacteristic.Get += HandleReadRequest;
            controlPointCharacteristic.Set += HandleWriteRequest;

            // add characteristics to setup service.

            // create setup service.
            AddService(new SetupService())
                .AddCharacteristic(new StateCharacteristic(0))
                .AddCharacteristic(new VersionCharacteristic("1.0"))
                .AddCharacteristic(controlPointCharacteristic);

            // set accessory information service characteristics
            GetService(typeof(AccessoryInformationService))
                .SetCharacteristic(typeof(ManufacturerCharacteristic), "Hüseyin Uslu")
                .SetCharacteristic(typeof(ModelCharacteristic), "Homer")
                .SetCharacteristic(typeof(SerialNumberCharacteristic), "CC:22:3D:E3:CE:30")
                .SetCharacteristic(typeof(FirmwareRevisionCharacteristic), "0.1");
        }


        private void HandleReadRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleWriteRequest(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
