using System;
using System.Collections.Generic;
using System.Text;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class SerialNumberCharacteristic : Characteristic
    {
        public SerialNumberCharacteristic() : base(
            "00000030-0000-1000-8000-0026BB765291",
            "Serial Number",
            CharacteristicFormat.String,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead
            })
        {
        }
    }
}
