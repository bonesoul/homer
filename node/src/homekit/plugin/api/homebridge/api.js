
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
const HomebridgeApiVersion = require('homekit/plugin/api/homebridge/version');

module.exports = class HomebridgePluginApi {
  constructor() {
    this._accessories = {};
    this._configurableAccessories = {};

    this._platforms = {};
    this._dynamicPlatforms = {};

    // expose the homebridge API version
    this.version = 2.4;

    // expose the homebridge server version
    this.serverVersion =  HomebridgeApiVersion.ApiCompatibilityVersion;

    // expose the User class methods to plugins to get paths. Example: homebridge.user.storagePath()
    this.user = user;

    // expose hap-nodejs.
    this.hap = hap;
  }

  registerAccessory = (pluginName, accessoryName, constructor, configurationRequestHandler) => {
    let fullName = `${pluginName}.${accessoryName}`;

    if (this._accessories[fullName])
      throw new Error(`[API] plugin ${pluginName} attempted to registern an accessory: ${accessoryName} which has been already registered!`);

    winston.verbose(`[API] registering accessory: ${pluginName}.${accessoryName}..`);

    this._accessories[fullName] = constructor;

    if (configurationRequestHandler)
      this._configurableAccessories[fullName] = configurationRequestHandler;
  }

  accessory = async (name) => {
    if (name.indexOf('.')  == -1 ) { // if we got a short name supplied
      let matches = [];

      // loop through all accessories and try matching ones.
      for(const fullName in this._accessories) {
        if (fullName.split(".")[1] == name)
        matches.push(fullName);
      }

      if (matches.length == 1) // if only found a single match
        return this._accessories[matches[0]]; // return it.
      else if (matches.length > 1) // if we found multiple matches
        throw new Error(`found multiple matches for given accessory name ${name}. Please expilicitly spesify by writing one of these; ${matches.join(', ')}`);
      else
        throw new Error(`can't find a matching accessory for given name ${name}`);
    } else { // if we got a full name in form of plugin.accessory notation.
      if (!this._accessories[name])
        throw new Error(`can't find a matching accessory for given name ${name}`);

      return this._accessories[name];
    }
  }

  registerPlatform = (pluginName, platformName, constructor, dynamic) => {
    let fullName = `${pluginName}.${platformName}`;

    if (this._platforms[fullName])
      throw new Error(`[API] plugin ${pluginName} attempted to registern an platform: ${platformName} which has been already registered!`);

      winston.verbose(`[API] registering platform: ${pluginName}.${platformName}..`);

      this._platforms[fullName] = constructor;

      if (dynamic)
        this._dynamicPlatforms[fullName] = constructor;
  }
}