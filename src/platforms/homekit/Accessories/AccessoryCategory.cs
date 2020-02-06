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
//      For purposes of the foregoing, “Sell” means practicing any or all of the rights granted to you under the License to provide to third
//      parties, for a fee or other consideration (including without limitation fees for hosting or consulting/ support services related to the
//      Software), a product or service whose value derives, entirely or substantially, from the functionality of the Software.Any license
//      notice or attribution required by the License must also include this Commons Clause License Condition notice.
//
//      License: MIT License
//      Licensor: Hüseyin Uslu
#endregion

namespace Homer.Platform.HomeKit.Accessories
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
