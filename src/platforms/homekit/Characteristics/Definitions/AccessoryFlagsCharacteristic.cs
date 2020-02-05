using System.Collections.Generic;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class AccessoryFlagsCharacteristic : Characteristic
    {
        public AccessoryFlagsCharacteristic() : base(
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
