using System.Collections.Generic;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class NameCharacteristic : Characteristic
    {
        public NameCharacteristic() : base(
            "00000023-0000-1000-8000-0026BB765291",
            "Name",
            CharacteristicFormat.String,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead
            })
        {
        }
    }
}
