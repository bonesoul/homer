using System.Collections.Generic;
using DaanV2.UUID;

namespace Smarty.Core.HomeKit.Characteristics
{
    /// <summary>
    /// A characteristic is a feature that represents data or an associated behavior of a service. The characteristic is defined
    /// by a universally unique type, and has additional properties that determine how the value of the characteristic can be
    /// accessed.
    /// </summary>
    public interface ICharacteristic
    {
        /// <summary>
        /// UUID of the characteristic.
        /// </summary>
        public UUID Uuid { get; }

        /// <summary>
        /// Display name of the characteristic.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Format
        /// </summary>
        public CharacteristicFormat Format { get; }

        /// <summary>
        /// Unit.
        /// </summary>
        public CharacteristicUnit Unit { get; }

        /// <summary>
        /// Permissions
        /// </summary>
        public IReadOnlyList<CharacteristicPermission> Permissions { get; }
    }
}
