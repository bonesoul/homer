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

        public string SavedCacheHash { get; }

        private Dictionary<string, int> _cache;

        public IdentifierCache(dynamic info)
        {
            _cache = new Dictionary<string, int>();
            Cache = new ReadOnlyDictionary<string, int>(_cache);

            Username = info.username;
            SavedCacheHash = "";
        }

        public int InstanceIdForAccessory(IAccessoryBase accessory)
        {
            if (accessory == null) throw new ArgumentNullException(nameof(accessory));

            var key = accessory.Uuid;
            return GetCache(key).HasValue
                ? GetCache(key).Value
                : SetCache(key, _cache.Count + 1);
        }

        public int GetInstanceIdForService(IAccessoryBase accessory, IService service)
        {
            if (accessory == null) throw new ArgumentNullException(nameof(accessory));
            if (service == null) throw new ArgumentNullException(nameof(service));

            var key = $"{accessory.Uuid}|{service.Uuid}";
            return GetCache(key).HasValue
                ? GetCache(key).Value
                : SetCache(key, _cache.Count + 1);
        }

        public int GetInstanceIdForCharacteristic(IAccessoryBase accessory, IService service, ICharacteristic characteristic)
        {
            if (accessory == null) throw new ArgumentNullException(nameof(accessory));
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (characteristic == null) throw new ArgumentNullException(nameof(characteristic));

            var key = $"{accessory.Uuid}|{service.Uuid}|{characteristic.Uuid}";
            return GetCache(key).HasValue
                ? GetCache(key).Value
                : SetCache(key, _cache.Count + 1);
        }

        private int? GetCache(string key)
        {
            if (_cache.ContainsKey(key)) return _cache[key];
            return null;
        }

        private int SetCache(string key, int value)
        {
            _cache[key] = value;
            return value;
        }
    }
}
