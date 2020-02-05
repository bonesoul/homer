using DaanV2.UUID;

namespace Smarty.Platform.HomeKit.Characteristics
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
        /// The value of the characteristic, which must conform to the ”format” property.The literal value null may also be used
        /// if the characteristic has no value.This property must be present if and only if the characteristic contains the
        /// Paired Read permission, see Table 6-4 (page 56).
        /// </summary>
        public dynamic Value { get; }
    }
}
