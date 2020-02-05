using System.Collections.Generic;

namespace Smarty.Platform.HomeKit.Characteristics
{
    public interface ICharacteristicProps
    {
        /// <summary>
        /// Array of permission strings describing the capabilities of the characteristic.
        /// </summary>
        public IReadOnlyList<CharacteristicPermission> Permissions { get; }

        /// <summary>
        /// (Optional)
        /// Boolean indicating if event notifications are enabled for this characteristic.
        /// </summary>
        public bool EventNotificationsEnabled { get; }

        /// <summary>
        /// (Optional)
        /// String describing the characteristic on a manufacturer-specific basis, such as an indoor versus outdoor temperature reading.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Format of the value, e.g. ”float”.
        /// </summary>
        public CharacteristicFormat Format { get; }

        /// <summary>
        /// Unit of the value, e.g. ”celsius”
        /// </summary>
        public CharacteristicUnit Unit { get; }

        /// <summary>
        /// (Optional)
        /// Minimum value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// </summary>
        public dynamic MinValue { get; }

        /// <summary>
        /// (Optional)
        /// Maximum value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// </summary>
        public dynamic MaxValue { get; }

        /// <summary>
        /// (Optional)
        /// Minimum step value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// For  example, if this were 0.15, the characteristic value can be incremented from the minimum value in multiples of 0.15.
        /// For “float”, the “Value” needs to be rounded on the accessory side to the closest allowed value per  the ”Step Value”
        /// (e.g.a value of 10.150001 received on the accessory side with a ”Step  Value” of 0.15 and a ”Minimum Value” of 10.0 needs
        /// to be interpreted as 10.15).
        /// </summary>
        public dynamic MinStep { get; }

        /// <summary>
        /// (Optional default: 64)
        /// Maximum number of characters if the format is ”string”. If this property is omitted for ”string” formats, then the default value is 64.
        /// The maximum value allowed is 256.
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// (Optional default: 2097152)
        /// Maximum number of characters if the format is ”data”. If this property is omitted for ”data” formats, then the default value is 2097152.
        /// </summary>
        public int MaxDataLength { get; }

        /// <summary>
        /// (Optional)
        /// An array of numbers where each element represents a valid value.
        /// </summary>
        public IList<dynamic> ValidValues { get; }

        /// <summary>
        /// (Optional)
        /// A 2 element array representing the starting value and ending value of the range of valid values.
        /// </summary>
        public dynamic[] ValidValuesRange { get; }
    }
}
