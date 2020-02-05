using System;
using System.Collections.Generic;
using DaanV2.UUID;
using Homer.Platform.HomeKit.Characteristics;
using Homer.Platform.HomeKit.Entity;

namespace Homer.Platform.HomeKit.Services
{
    /// <summary>
    /// Interface for HomeKit services.
    /// </summary>
    public interface IService: IEntity
    {
        public IReadOnlyDictionary<Type, ICharacteristic> Characteristics { get; }

        public IService SetCharacteristic(Type t, dynamic value);
    }
}
