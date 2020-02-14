using Homer.Platform.HomeKit.Characteristics.Definitions;

namespace Homer.Platform.HomeKit.Services.Definitions
{
    public class ProtocolInformationService : Service
    {
        public ProtocolInformationService()
            : base("000000A2-0000-1000-8000-0026BB765291", "Protocol Information")
        {
            // required characteristics.
            AddCharacteristic(typeof(VersionCharacteristic));
        }
    }
}