using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Homer.Platform.HomeKit.Accessories;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Services;

namespace Homer.Platform.HomeKit.Caches.Identifiers
{
    public class IdentifierCache : IIdentifierCache
    {
        public string Username { get; }

        public IReadOnlyDictionary<string, int> Cache { get; }

        public IReadOnlyDictionary<string, int> UsedCache { get; }

        public string SavedCacheHash { get; }

        private Dictionary<string, int> _cache;

        private Dictionary<string, int> _usedCache;

        public IdentifierCache(dynamic info)
        {
            _cache = new Dictionary<string, int>();
            _usedCache = new Dictionary<string, int>();

            Cache = new ReadOnlyDictionary<string, int>(_cache);
            UsedCache = new ReadOnlyDictionary<string, int>(_usedCache);

            Username = info.username;
            SavedCacheHash = "";
        }

        public string GetInstanceId(IAccessoryBase accessory, IService service, ICharacteristic characteristic)
        {
            if (accessory == null) throw new ArgumentNullException(nameof(characteristic));
            if (service == null) throw new ArgumentNullException(nameof(characteristic));
            if (characteristic == null) throw new ArgumentNullException(nameof(characteristic));

            return $"{accessory.Uuid}|{service.Uuid}|{characteristic.Uuid}";
;        }
    }
}
