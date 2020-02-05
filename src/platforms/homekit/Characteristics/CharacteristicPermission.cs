namespace Homer.Platform.HomeKit.Characteristics
{
    /// <summary>
    /// Characteristic permissions
    /// </summary>
    public enum CharacteristicPermission
    {
        /// <summary>
        /// This characteristic can only be read by paired controllers.
        /// </summary>
        PairedRead,
        /// <summary>
        /// This characteristic can only be written by paired controllers.
        /// </summary>
        PairedWrite,
        /// <summary>
        /// This characteristic supports events. The HAP Characteristic object
        /// must contain the ”ev” key if it supports events.
        /// </summary>
        Events,
        /// <summary>
        /// This characteristic supports additional authorization data
        /// </summary>
        AdditionalAuthorization,
        /// <summary>
        /// This characteristic allows only timed write procedure
        /// </summary>
        TimedWrite,
        /// <summary>
        /// This characteristic is hidden from the user
        /// </summary>
        Hidden,
        /// <summary>
        /// This characteristic supports write response
        /// </summary>
        WriteResponse
    }
}
