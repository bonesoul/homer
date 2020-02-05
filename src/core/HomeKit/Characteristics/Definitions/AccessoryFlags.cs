using System;
using System.Collections.Generic;
using System.Text;

namespace Smarty.Core.HomeKit.Characteristics.Definitions
{
    public class AccessoryFlags : Characteristic
    {
        public AccessoryFlags() : base(
            "000000A6-0000-1000-8000-0026BB765291", 
            "Accessory Flags",
            CharacteristicFormat.Uint32,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead,
                CharacteristicPermission.Events
            })
        {
        }
    }
}
