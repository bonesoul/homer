namespace Smarty.Core.HomeKit.Characteristics
{
    // Data presentation formats.
    public enum CharacteristicFormat
    {
        Bool, // unsigned 8-bit; 0 = false, 1 = true
        Int, // signed 32-bit integer
        Float, // IEEE-754 32-bit floating point
        String, // UTF-8 string
        Uint8, // unsigned 8-bit integer
        Uint16, // unsigned 16-bit integer
        Uint32, // unsigned 32-bit integer
        Uint64, // unsigned 64-bit integer
        Data, // Opaque structure
        Tlv8 // Opaque structure
    }
}
