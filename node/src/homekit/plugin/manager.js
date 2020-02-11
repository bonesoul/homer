
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
const path = require('path');
const fs = require('fs-extra')
const _ = require('lodash');
const globalDirectories = require('global-dirs');

module.exports = class PluginManager {
  discover = async () => {
    let discovered = [];
    let searchedPaths = {};
    let paths = await this._getPaths();

    winston.verbose(`[PLUGIN_MANAGER] discovering plugins..`);

    for (const path of paths) {
      if (searchedPaths[path]) return;
      searchedPaths[path] = true;

      let plugins = await this._discoverPath(path);

      if (Object.keys(plugins).length === 0)
        continue;

      Object.entries(plugins).forEach(([key, value]) => {
        winston.verbose(`[PLUGIN_MANAGER] discovered plugin: ${key}..`);
        discovered[key] = value;
      });
    }

    return discovered;
  }

  load = async(name, dir) => {
    winston.info(`[PLUGIN_MANAGER] loading plugin ${name}..`);
  }

  _discoverPath = async (dir) => {
    try {
      winston.verbose(`[PLUGIN_MANAGER] checking ${dir} for plugins..`);

      let plugins = [];

      // check the directory.
      if (!await fs.exists(dir))
        return plugins;

      // read subdirectories.
      let names = await fs.readdir(dir);

      // if this is a module directory with package.json, just skip it.
      if (await fs.exists(path.join(dir, "package.json")))
        names = [""];

      for (const name of names) {
        let pluginPath = path.join(dir, name);
        var plugin = await this._checkPlugin(pluginPath);
        if (!plugin) continue;

        plugins[plugin.name] = pluginPath;
      }

      return plugins;
    } catch (err) {
      if (err.errno !== -4058) winston.error(err); // -4058 is just path not found error.
    }
  }

  _checkPlugin = async (dir) => {
    let stat = await fs.stat(dir);
    if (!stat.isDirectory()) return; // make sure it's a directory.

    let json = await this._loadPluginJson(dir); // try loading the package.json of the plugin.
    return json;
  }

  _loadPluginJson = async (dir) => {
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

  _getPaths = async () => {
    let paths = [];

    paths = paths.concat(require.main.paths); // add require paths.
    paths.push(path.join(globalDirectories.npm.packages));

    return paths;
  }
}