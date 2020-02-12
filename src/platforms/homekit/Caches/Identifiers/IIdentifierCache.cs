using System.Collections.Generic;
using Homer.Platform.HomeKit.Accessories;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Services;

namespace Homer.Platform.HomeKit.Caches.Identifiers
{
    /// <summary>
    /// IdentifierCache is a model class that manages a system of associating HAP "Accessory IDs" and "Instance IDs"
    /// with other values that don't usually change. HomeKit Clients use Accessory/Instance IDs as a primary key of
    /// sorts, so the IDs need to remain "stable". For instance, if you create a HomeKit "Scene" called "Leaving Home"
    /// that sets your Alarm System's "Target Alarm State" Characteristic to "Arm Away", that Scene will store whatever
    /// "Instance ID" it was given for the "Target Alarm State" Characteristic.If the ID changes later on this server,
    /// the scene will stop working.
    /// </summary>
    public interface IIdentifierCache
    {
        string Username { get; }

        IReadOnlyDictionary<string, int> Cache { get; }

        string SavedCacheHash { get; }

        int GetInstanceIdForAccessory(IAccessoryBase accessory);

        int GetInstanceIdForService(IAccessoryBase accessory, IService service);

        int GetInstanceIdForCharacteristic(IAccessoryBase accessory, IService service, ICharacteristic characteristic);
    }
}
