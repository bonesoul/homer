using System;
using System.Collections.Generic;
using System.Text;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class IdentityCharacteristic : Characteristic
    {
        public IdentityCharacteristic() : base(
            "00000014-0000-1000-8000-0026BB765291",
            "Identify",
            CharacteristicFormat.Bool,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.WriteResponse
            })
        {
        }
    }
}
