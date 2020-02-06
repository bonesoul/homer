#region license
// 
//     homer - The complete home automation for Homer Simpson.
//     Copyright (C) 2020, Hüseyin Uslu - shalafiraistlin at gmail dot com
//     https://github.com/bonesoul/homer
// 
//      “Commons Clause” License Condition v1.0
//
//      The Software is provided to you by the Licensor under the License, as defined below, subject to the following condition.
//  
//      Without limiting other conditions in the License, the grant of rights under the License will not include, and the License
//      does not grant to you, the right to Sell the Software.
//
//      For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide
//      to third parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support
//      services related to the Software), a product or service whose value derives, entirely or substantially, from the functionality
//      of the Software.Any license notice or attribution required by the License must also include this Commons Clause License
//      Condition notice.
//
//      License: MIT License
//      Licensor: Hüseyin Uslu
#endregion

using System.Collections.Generic;

namespace Homer.Platform.HomeKit.Characteristics
{
    public interface ICharacteristicProps
    {
        /// <summary>
        /// Array of permission strings describing the capabilities of the characteristic.
        /// </summary>
        IReadOnlyList<CharacteristicPermission> Permissions { get; }

        /// <summary>
        /// (Optional)
        /// Boolean indicating if event notifications are enabled for this characteristic.
        /// </summary>
        bool EventNotificationsEnabled { get; }

        /// <summary>
        /// (Optional)
        /// String describing the characteristic on a manufacturer-specific basis, such as an indoor versus outdoor temperature reading.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Format of the value, e.g. ”float”.
        /// </summary>
        CharacteristicFormat Format { get; }

        /// <summary>
        /// Unit of the value, e.g. ”celsius”
        /// </summary>
        CharacteristicUnit Unit { get; }

        /// <summary>
        /// (Optional)
        /// Minimum value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// </summary>
        dynamic MinValue { get; }

        /// <summary>
        /// (Optional)
        /// Maximum value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// </summary>
        dynamic MaxValue { get; }

        /// <summary>
        /// (Optional)
        /// Minimum step value for the characteristic, which is only appropriate for characteristics that have a format of ”int” or ”float”.
        /// For  example, if this were 0.15, the characteristic value can be incremented from the minimum value in multiples of 0.15.
        /// For “float”, the “Value” needs to be rounded on the accessory side to the closest allowed value per  the ”Step Value”
        /// (e.g.a value of 10.150001 received on the accessory side with a ”Step  Value” of 0.15 and a ”Minimum Value” of 10.0 needs
        /// to be interpreted as 10.15).
        /// </summary>
        dynamic MinStep { get; }

        /// <summary>
        /// (Optional default: 64)
        /// Maximum number of characters if the format is ”string”. If this property is omitted for ”string” formats, then the default value is 64.
        /// The maximum value allowed is 256.
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// (Optional default: 2097152)
        /// Maximum number of characters if the format is ”data”. If this property is omitted for ”data” formats, then the default value is 2097152.
        /// </summary>
        int MaxDataLength { get; }

        /// <summary>
        /// (Optional)
        /// An array of numbers where each element represents a valid value.
        /// </summary>
        IList<dynamic> ValidValues { get; }

        /// <summary>
        /// (Optional)
        /// A 2 element array representing the starting value and ending value of the range of valid values.
        /// </summary>
        dynamic[] ValidValuesRange { get; }
    }
}
