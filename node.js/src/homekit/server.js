
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

const Bridge = require("hap-nodejs").Bridge;
const Accessory = require("hap-nodejs").Accessory;
const Service = require("hap-nodejs").Service;
const Characteristic = require("hap-nodejs").Characteristic;
const AccessoryLoader = require("hap-nodejs").AccessoryLoader;
const once = require("hap-nodejs").once;
const Uuid = require("hap-nodejs").uuid;
const winston = require('winston');
const config = require('config');
const qrcode = require('qrcode-terminal');
const chalk = require('chalk');
const user = require('lib/user');
const storage = require('node-persist');
const PluginManager = require('homekit/plugin/manager');
const PlatformRepository = require('homekit/repository/platform');
const AccessoryRepository = require('homekit/repository/accessory');
const HomebridgePluginApi = require('homekit/plugin/api/homebridge/api');
const packageInfo = require('../../package.json');

module.exports = class Server {
  constructor() {
    return (async () => {
      this._cleanCachedAccessories = false;
      this._allowInsecureAccess = config.get('homekit.setup.insecure'); // should be only allowed for debugging purposes as this will allow unauthenticated requests.

      this._bridge = await this._createBridge();
      this._pluginApi = new HomebridgePluginApi();
      this._pluginManager = new PluginManager(this._pluginApi);
      this._accessoryRepository = new AccessoryRepository(this._pluginApi, this._bridge);
      this._platformRepository = new PlatformRepository(this._pluginApi, this._accessoryRepository, this._bridge);

      // init accessory storage.
      winston.verbose(`initializing accessory storage over path ${user.cachedAccessoryPath()}`, { label: 'server'});
      await storage.init({ dir: user.cachedAccessoryPath() });

      await this._pluginManager.discover(); // discover plugins.
      await this._pluginManager.load(); // load plugins.
      await this._pluginManager.initialize(); // initialize plugins.

      this._bridge.on('listening', function(port) {
        winston.info(`homer is running on port ${port}.`, { label: 'server'});
      });

      return this;
    })();
  }

  run = async () => {
    if (config.get('homekit.platforms')) await this._platformRepository.load();
    if (config.get('homekit.accessories')) await this._accessoryRepository.load();

    await this._publish();

    // this._orchestrate();
  }

  _orchestrate = async () => {
    // get hue bridge.
    let huePlatform = this._platformRepository.active['Hue.Hue'];

    // wait for hue bridge to expose the bulbs.
    let lights = await this._getLights(huePlatform);

    // start listening for plex events.
    this._accessoryRepository.active['Plex.Plex'].getService(Service.OccupancySensor).getCharacteristic(Characteristic.OccupancyDetected).on('change', (data) => {
        for(const entry of lights) { // loop through all lights.
          entry.lightService.getCharacteristic(Characteristic.On).setValue(!data.newValue); // if plex started streaming close them, otherwise re-open them.
        }
    });
  }

  _getLights = async (hue) => {
    return new Promise((resolve, reject) => {
      try {
        let lights = [];
        hue.accessories((accessories) => {
          for(const entry of accessories) {
            if (entry.constructor.name === 'HueAccessory' && entry.lightService) {
              lights.push(entry);
            }
          }
          return resolve(lights);
        });
      } catch (err) {
        return reject(err);
      }
    });
  }

  _publish = async () => {
    let accessoryInformationService = this._bridge.getService(Service.AccessoryInformation)
      .setCharacteristic(Characteristic.Manufacturer, packageInfo.author.name)
      .setCharacteristic(Characteristic.Model, packageInfo.name)
      .setCharacteristic(Characteristic.SerialNumber, config.get('homekit.setup.serial'))
      .setCharacteristic(Characteristic.FirmwareRevision, packageInfo.version);

    var publishInfo = {
      username: config.get('homekit.setup.serial'),
      port: config.get('homekit.setup.port'),
      pincode: config.get('homekit.setup.pin'),
      category: Accessory.Categories.BRIDGE,
    }

    winston.verbose(`publishing ${publishInfo.username} over port ${publishInfo.port} using pin ${publishInfo.pincode}..`, { label: 'server'});

    await this._bridge.publish(publishInfo, this._allowInsecureAccess);
    await this._printSetupInfo();
  }

  _createBridge = async () => {
    var uuid = Uuid.generate('homer');
    winston.verbose(`creating bridge: homer, uuid: ${uuid}`, { label: 'server'})
    return new Bridge('homer', uuid);
  }

  _printSetupInfo = async () => {
    winston.info(`setup payload ${this._bridge.setupURI()}`, { label: 'server'});
    winston.info(`scan this code with your Homekit device to pair..`, { label: 'server'});
    qrcode.generate(this._bridge.setupURI());
    winston.info(`or enter this pin on your Homekit device to pair..`, { label: 'server'});
    winston.info(chalk.black.bgYellow(`                       `), { label: 'server'});
    winston.info(chalk.black.bgYellow(`    ┌────────────┐     `), { label: 'server'});
    winston.info(chalk.black.bgYellow(`    │ ${config.get('homekit.setup.pin')} │     `), { label: 'server'});
    winston.info(chalk.black.bgYellow(`    └────────────┘     `), { label: 'server'});
    winston.info(chalk.black.bgYellow(`                       `), { label: 'server'});
  }
}
