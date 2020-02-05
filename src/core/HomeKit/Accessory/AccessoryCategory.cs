namespace Smarty.Core.HomeKit.Accessory
{
    /// <summary>
    /// Accessory categories
    /// <remarks>
    /// Note: An accessory with support for multiple categories should advertise the primary category. An accessory for which a primary
    /// category cannot be determined or the primary category isnʼt among the well defined categories(2-9) falls in the Other category.
    /// See: HomeKit Accessory Protocol Specification Non-Commercial Version Release R2, Section 13.
    /// </remarks>
    /// </summary>
    public enum AccessoryCategory
    {
        Other = 1,
        Bridge = 2,
        Fan = 3,
        GarageDoorOpener = 4,
        Lightning = 5,
        Lock = 6,
        Outlet = 7,
        Switch = 8,
        Thermostat = 9,
        Sensor = 10,
        SecuritySystem = 11,
        Door = 12,
        Window = 13,
        WindowCovering = 14,
        ProgrammableSwitch = 15,
        RangeExtender = 16,
        IpCamera = 17,
        VideoDoorbell = 18,
        AirPurifier = 19,
        Heater = 20,
        AirConditioner = 21, 
        Humidifier = 22,
        Dehumidifier = 23, 
        AppleTv = 24, // AppleTv
        HomePod = 25, // Apple HomePod
        Speaker = 26,
        Airport = 27,
        Sprinkler = 28,
        Faucet = 29,
        ShowerHead = 30,
        Television = 31,
        Remote = 32, // Remote Control
        Router = 33 // HomeKit enabled router
    }
}
