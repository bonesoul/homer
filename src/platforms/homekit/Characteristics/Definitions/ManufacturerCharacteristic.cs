using System;
using System.Collections.Generic;
using System.Text;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class ManufacturerCharacteristic : Characteristic
    {
        public ManufacturerCharacteristic() : base(
            "00000020-0000-1000-8000-0026BB765291",
            "Manufacturer",
            CharacteristicFormat.String,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead
            })
        {
        }
    }
}
