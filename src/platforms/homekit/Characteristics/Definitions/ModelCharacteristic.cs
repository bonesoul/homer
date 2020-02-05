using System;
using System.Collections.Generic;
using System.Text;

namespace Homer.Platform.HomeKit.Characteristics.Definitions
{
    public class ModelCharacteristic : Characteristic
    {
        public ModelCharacteristic() : base(
            "00000021-0000-1000-8000-0026BB765291",
            "Model",
            CharacteristicFormat.String,
            new List<CharacteristicPermission>()
            {
                CharacteristicPermission.PairedRead
            })
        {
        }
    }
}
