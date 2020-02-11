
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

const hap = require("hap-nodejs");
const winston = require('winston');
const user = require('lib/user');

module.exports = class PluginApi {
  constructor() {
    this._accessories = {};
    this._configurableAccessories = {};

    this._platforms = {};
    this._dynamicPlatforms = {};

    // expose the homebridge API version
    this.version = 2.4;

    // expose the homebridge server version
    this.serverVersion =  "0.4.5";

    // expose the User class methods to plugins to get paths. Example: homebridge.user.storagePath()
    this.user = user;

    // expose hap-nodejs.
    this.hap = hap;
  }

  registerAccessory = (pluginName, accessoryName, constructor, configurationRequestHandler) => {
    let name = `${pluginName}.${accessoryName}`;

    if (this._accessories[name])
      throw new Error(`plugin ${pluginName} attempted to registern an accessory: ${accessoryName} which has been already registered!`);

    winston.info(`[PLUGIN:${pluginName}] registering accessory: ${accessoryName}..`);
  }
}