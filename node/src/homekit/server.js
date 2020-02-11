
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
const Uuid = require("hap-nodejs").uuid;
const winston = require('winston');
const config = require('config');
const qrcode = require('qrcode-terminal');
const chalk = require('chalk');
const user = require('lib/user');
const PluginApi = require('homekit/plugin/api/api');
const accessoryStorage = require('node-persist').create();
const packageInfo = require('../../package.json');

module.exports = class Server {

  constructor(cleanCachedAccessories = false) {
    this._cleanCachedAccessories = cleanCachedAccessories;
    this._allowInsecureAccess = config.get('platforms.homekit.setup.insecure'); // should be only allowed for debugging purposes as this will allow unauthenticated requests.

    // init accessory storage.
    winston.verbose(`initializing accessory storage over path ${user.cachedAccessoryPath()}`);
    accessoryStorage.initSync({ dir: user.cachedAccessoryPath() });

    // init plugin apis.
    this._pluginApi = new PluginApi();

    // load plugins.
    this._loadPlugins();

    this._bridge = this._createBridge();
    this._bridge.on('listening', function(port) {
      winston.info(`homer is running on port ${port}.`);
    });
  }

  run = () => {
    this._publish();
  }

  _publish = () => {
    let accessoryInformationService = this._bridge.getService(Service.AccessoryInformation)
      .setCharacteristic(Characteristic.Manufacturer, packageInfo.author.name)
      .setCharacteristic(Characteristic.Model, packageInfo.name)
      .setCharacteristic(Characteristic.SerialNumber, config.get('platforms.homekit.setup.serial'))
      .setCharacteristic(Characteristic.FirmwareRevision, packageInfo.version);

    var publishInfo = {
      username: config.get('platforms.homekit.setup.serial'),
      port: config.get('platforms.homekit.setup.port'),
      pincode: config.get('platforms.homekit.setup.pin'),
      category: Accessory.Categories.BRIDGE,
    }

    winston.verbose(`[SERVER] publishing ${publishInfo.username} over port ${publishInfo.port} using pin ${publishInfo.pincode}..`);

    this._bridge.publish(publishInfo, this._allowInsecureAccess);
    this._printSetupInfo();
  }

  _loadPlugins = () => {
    winston.verbose('loading plugins..');
  };

  _createBridge = () => {
    var uuid = Uuid.generate('homer');
    winston.verbose(`[SERVER] creating bridge: homer, uuid: ${uuid}`)
    return new Bridge('homer', uuid);
  }

  _printSetupInfo = () => {
    winston.info(`[SERVER] setup payload ${this._bridge.setupURI()}`);
    winston.info(`[SERVER] scan this code with your Homekit device to pair..`);
    qrcode.generate(this._bridge.setupURI());
    winston.info(`[SERVER] or enter this pin on your Homekit device to pair..`);
    winston.info(chalk.black.bgYellow(`                       `));
    winston.info(chalk.black.bgYellow(`    ┌────────────┐     `));
    winston.info(chalk.black.bgYellow(`    │ ${config.get('platforms.homekit.setup.pin')} │     `));
    winston.info(chalk.black.bgYellow(`    └────────────┘     `));
    winston.info(chalk.black.bgYellow(`                       `));
  }
}
