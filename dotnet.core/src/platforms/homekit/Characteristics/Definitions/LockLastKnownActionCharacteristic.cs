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

using System.Collections.Generic;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class LockLastKnownActionCharacteristic: Characteristic
    {
      public const string Uuid = "0000001C-0000-1000-8000-0026BB765291";

    
        public const int Secured_Physically_Interior = 0;
        public const int Unsecured_Physically_Interior = 1;
        public const int Secured_Physically_Exterior = 2;
        public const int Unsecured_Physically_Exterior = 3;
        public const int Secured_By_Keypad = 4;
        public const int Unsecured_By_Keypad = 5;
        public const int Secured_Remotely = 6;
        public const int Unsecured_Remotely = 7;
        public const int Secured_By_Auto_Secure_Timeout = 8;

        public LockLastKnownActionCharacteristic(): base(
            uuid: "0000001C-0000-1000-8000-0026BB765291",
            displayName: "Lock Last Known Action",
            format: CharacteristicFormat.Uint8,
            
            
            
            
            validValues: new List<int> {0,1,2,3,4,5,6,7,8,},
            permissions: new List<CharacteristicPermission>
            {
                CharacteristicPermission.PairedRead,
                CharacteristicPermission.Events,
                
                
            })
        {
        }
    }
}