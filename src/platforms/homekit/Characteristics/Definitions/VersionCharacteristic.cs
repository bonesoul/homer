using System;
using System.Collections.Generic;
using System.Text;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class VersionCharacteristic:Characteristic
    {
        public VersionCharacteristic() : base(
            "00000037-0000-1000-8000-0026BB76529",
            "Version",
            CharacteristicFormat.String,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead
            })
        {
        }
    }
}
