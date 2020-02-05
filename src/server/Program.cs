using System;
using System.Threading.Tasks;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Characteristics.Definitions;
using Homer.Platform.HomeKit.Services;

namespace Homer.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var uuid = UUIDFactory.CreateUUID(4, 2);
                var service = new Service(uuid, "test")
                    .SetCharacteristic(typeof(ManufacturerCharacteristic), "Default - Manufacturer")
                    .SetCharacteristic(typeof(ModelCharacteristic), "Default-Model")
                    .SetCharacteristic(typeof(SerialNumberCharacteristic), "Default-SerialNumber");
            }
            catch (Exception e)
            {

            }
        }
    }
}
