
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
'use strict';

const winston = require('winston');
const config = require('config');
const uuid = require("hap-nodejs").uuid;
const Accessory = require("hap-nodejs").Accessory;
const Service = require("hap-nodejs").Service;
const Characteristic = require("hap-nodejs").Characteristic;

module.exports = class AccessoryRepository {
  constructor(pluginApi, bridge) {
    this.active = {}
    this._pluginApi = pluginApi;
    this._bridge = bridge;
  }

  load = async () => {
    winston.verbose('loading accessories..', { label: 'accessoryrep'});

    for (const entry of config.get('homekit.accessories')) {
      await this._loadAccessory(entry);
    }
  }

  _loadAccessory = async(accessoryConfig) => {
    try {
      var type = accessoryConfig.accessory;
      var name = accessoryConfig.name;

      var ctor = await this._pluginApi.accessory(type); // get accessory's ctor.
      if (!ctor) throw new Error(`requested unregistered accessory ${type}`, { label: 'accessoryrep'});

      const logger = require('lib/logger/logger').customLogger('accessory', type, name); // create custom logger for accessory
      var instance = new ctor(logger, accessoryConfig); // create an instance.
      var accessory = await this.createAccessory(instance, name, type, accessoryConfig.uuid_base); // create accessory.

      this._bridge.addBridgedAccessory(accessory); // add accessory to our bridge.
    } catch (err) {
      winston.error(`error loading accessory; ${err.stack}`, { label: 'accessoryrep'})
    }
  }

  createAccessory = async(accessoryInstance, displayName, accessoryType, uuid_base) => {

    winston.verbose(`creating accessory: ${accessoryType} => ${displayName}..`, { label: 'accessoryrep'});

    var services = accessoryInstance.getServices();

    if (!(services[0] instanceof Service)) { // old homebridge api style.
      // Create the actual HAP-NodeJS "Accessory" instance
      return AccessoryLoader.parseAccessoryJSON({
        displayName: displayName,
        services: services
      });
    } else { // new homebridge api style.
      var accessoryUUID = uuid.generate(`${accessoryType}:${(uuid_base || displayName)}`);
      var accessory = new Accessory(displayName, accessoryUUID);

      // listen for the identify event if the accessory instance has defined an identify() method
      if (accessoryInstance.identify)
        accessory.on('identify', function(paired, callback) { accessoryInstance.identify(callback); });

      for(const service of services) {
        if (service instanceof Service.AccessoryInformation) { // accessory information service needs special handling.
          var existingService = accessory.getService(Service.AccessoryInformation);

          // get the values defined by plugin's accesory information service.
          var manufacturer = service.getCharacteristic(Characteristic.Manufacturer).value;
          var model = service.getCharacteristic(Characteristic.Model).value;
          var serialNumber = service.getCharacteristic(Characteristic.SerialNumber).value;
          var firmwareRevision = service.getCharacteristic(Characteristic.FirmwareRevision).value;
          var hardwareRevision = service.getCharacteristic(Characteristic.HardwareRevision).value;

          // set values overriden by plugin's accessory information service.
          if (manufacturer) existingService.setCharacteristic(Characteristic.Manufacturer, manufacturer);
          if (model) existingService.setCharacteristic(Characteristic.Model, model);
          if (serialNumber) existingService.setCharacteristic(Characteristic.SerialNumber, serialNumber);
          if (firmwareRevision) existingService.setCharacteristic(Characteristic.FirmwareRevision, firmwareRevision);
          if (hardwareRevision) existingService.setCharacteristic(Characteristic.HardwareRevision, hardwareRevision);
        } else {
          accessory.addService(service);
        }
      }

      this.active[`${accessoryType}.${displayName}`] = accessory;

      return accessory;
    }
  }
};