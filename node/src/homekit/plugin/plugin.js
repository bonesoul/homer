
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

const path = require('path');
const winston = require('winston');
const fs = require('fs-extra')
const semver = require('semver');

const homebridge_compatiblity_version = "0.4.5";

module.exports = class Plugin {
  constructor(name, dir) {
    this.name = name;
    this.dir = dir;
    this.initializer = undefined; // will be later filled by load().
  }

  load = async() => {
    winston.info(`[PLUGIN:${this.name}] loading plugin..`);

    var json = await Plugin.getJson(this.dir);

    // make sure it has a valid json.
    if (json === undefined) 
      throw new Error(`[PLUGIN:${this.name}] plugin does not have a valid package.json..`);

    // check if it has homebridge or homer as engine.
    if (!json.engines || (!json.engines.homebridge && !json.engines.homer))
      throw new Error(`[PLUGIN:${this.name}] plugin does not contain correct engines definitions..`);

    // check if homer version is satisfied.
    if (json.engines.homer && !semver.satisfies(packageInfo.version, json.engines.homer) )
      throw new Error(`[PLUGIN:${this.name}] plugin requires homer version ${json.engines.homer} which is not satisfied by current version ${packageInfo.version}. Please consider upgrading your homer installation..`);

    // check if homebridge version is satisfied.
    if (json.engines.homebridge && !semver.satisfies(homebridge_compatiblity_version, json.engines.homebridge) ) 
      throw new Error(`[PLUGIN:${this.name}] plugin requires homebridge compatability version ${json.engines.homebridge} which is not satisfied by current version ${homebridge_compatiblity_version}. Please consider upgrading your homer installation..`);

    // check node version.
    if (json.engines.node && !semver.satisfies(process.version, json.engines.node)) 
      winston.warn(`[PLUGIN:${this.name}] plugin requires node version ${json.engines.node} which is not satisfied by current version ${process.version}. Consider upgrading your node installation..`);

    // get plugin entrance
    let entrance = json.main || "./index.js";
    let entrancePath = path.join(this.dir, entrance);

    // try getting the plugin initializer.
    var module = require(entrancePath);

    if (typeof module === "function")
      this.initializer = module;
    else if (module && typeof module.default === "function")
      this.initializer = module.default;
    else
      throw new Error(`[PLUGIN:${this.name}] plugin does not export an initializer..`)
  }

  initialize = async (api) => {
    try {
      winston.info(`[PLUGIN:${this.name}] initializing plugin..`);
      this.initializer(api);
    } catch (err) {
      throw new Error(`[PLUGIN:${this.name}] error initializing plugin ${err.stack}`)
    }
  }

  static getJson = async (dir) => {
    let packageJsonPath = path.join(dir, "package.json");

    if (!await fs.exists(packageJsonPath))
      return;

    try {
      let file = await fs.readFile(packageJsonPath); // read the package.json.
      let json = await JSON.parse(file); // try parsing it.

      // make sure the plugin name starts with homebridge- or homer-
      if (!json.name || (json.name.indexOf('homebridge-') !== 0 && json.name.indexOf('homer-') !== 0))
        return;

      // verify the plugin is correctly tagged with homebridge-plugin or homer-plugin.
      if (!json.keywords || (json.keywords.indexOf("homebridge-plugin") == -1 && json.keywords.indexOf("homer-plugin") == -1))
        return;

      return json;
    } catch (err) {
      return;
    }
  }
};