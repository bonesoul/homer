
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
const once = require("hap-nodejs").once;

module.exports = class PlatformRepoistory {
  constructor(pluginApi, accessoryRepository, bridge) {
    this.active = {}
    this._pluginApi = pluginApi;
    this._accessoryRepository = accessoryRepository;
    this._bridge = bridge;
  }

  load = async () => {
    winston.verbose('loading platforms..', { label: 'platformrep'});
      for (const entry of config.get('homekit.platforms')) {
        await this._loadPlatform(entry);
      }
  }

  _loadPlatform = async(platformConfig) => {
    try {
      var type = platformConfig.platform;
      var name = platformConfig.name || type;

      var ctor = await this._pluginApi.platform(type); // get platform's ctor.
      if (!ctor) throw new Error(`requested unregistered accessory ${type}`, { label: 'platformrep'});

      const logger = require('lib/logger/logger').pluginLogger('platform', type, name); // create custom logger for platform
      var instance = new ctor(logger, platformConfig, this._pluginApi);

      if (instance.configureAccessory == undefined)
        this._loadPlatformAccessories(instance, type);
      else
        winston.warn('implement me!');

      this.active[`${type}.${name}`] = instance;

    } catch (err) {
      winston.error(`error loading accessory; ${err.stack}`, { label: 'platformrep'})
    }
  }

  _loadPlatformAccessories  = async (platformInstance, platformType) => {
    platformInstance.accessories(once(function(accessories){
      return (async () => {
        for(const accessoryInstance of accessories) {
          let name = accessoryInstance.name;
          var accessory = await this._accessoryRepository.createAccessory(accessoryInstance, name, platformType, accessoryInstance.uuid_base); // create platform supplied accessory.

          this._bridge.addBridgedAccessory(accessory); // add accessory to our bridge.
        }
      })();
    }.bind(this)));
  }
};