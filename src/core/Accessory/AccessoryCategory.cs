using System;
using System.Collections.Generic;
using System.Text;

namespace Smarty.Core.Accessory
{
    public enum AccessoryCategory
    {
        Other = 1,
        Bridge = 2,
        Fan = 3,
        GarageDoorOpener = 4,
        LightBulb = 5,
        DoorLock = 6,
        Outlet = 7,
        Switch = 8,
        Thermostat = 9,
        Sensor = 10,
        AlarmSystem = 11,
        SecuritySystem = 11, //Added to conform to HAP naming
        Door = 12,
        Window = 13,
        WindowCovering = 14,
        ProgrammableSwitch = 15,
        RangeExtender = 16,
        Camera = 17,
        IpCamera = 17, //Added to conform to HAP naming
        VideoDoorbell = 18,
        AirPurifier = 19,
        AirHeater = 20, //Not in HAP Spec
        AirConditioner = 21, //Not in HAP Spec
        AirHumidifier = 22, //Not in HAP Spec
        AirDehumidifier = 23, // Not in HAP Spec
        AppleTv = 24,
        HomePod = 25, // HomePod
        Speaker = 26,
        Airport = 27,
        Sprinkler = 28,
        Faucet = 29,
        ShowerHead = 30,
        Television = 31,
        TargetController = 32, // Remote Control
        Router = 33 // HomeKit enabled router
    }
}
