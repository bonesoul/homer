using System;
using System.Collections.Generic;
using System.Text;
using DaanV2.UUID;

namespace Homer.Platform.HomeKit.Entity
{
    public interface IEntity
    {
        /// <summary>
        /// UUID of the entity.
        /// </summary>
        public UUID Uuid { get; }

        /// <summary>
        /// Display name of the entity.
        /// </summary>
        public string DisplayName { get; }
    }
}
