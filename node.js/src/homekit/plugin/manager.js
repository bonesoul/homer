
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
const globalDirectories = require('global-dirs');
const Plugin = require('homekit/plugin/plugin');
const chalk = require('chalk');
const versions = require('homekit/plugin/api/versions');

module.exports = class PluginManager {
  constructor(pluginApi) {
    this._pluginApi = pluginApi;
    winston.info(`api compatibilities levels; homer: ${versions.Homer.ServerVersion} api: ${versions.Homer.ApiVersion}, homebridge: ${versions.Homebridge.ServerCompatibilityVersion} api: ${versions.Homebridge.ApiCompatibilityVersion}..`, { label: 'pluginmanager'})
  }

  discover = async() => {
    winston.verbose('loading plugins..', { label: 'pluginmanager'});

    this._plugins = await this._discover();

    this._plugins.length > 0
      ? winston.info(`discovered a total of ${this._plugins.length} plugins..`, { label: 'pluginmanager'})
      : winston.warn(`no plugins found. See the README for information on installing plugins..`, { label: 'pluginmanager'})
  }

  load = async() => {
    for (const plugin of this._plugins) {
      await plugin.load();
    }
  }

  initialize = async() => {
    for (const plugin of this._plugins) {
      await plugin.initialize(this._pluginApi);
    }
  };

  _discover = async () => {
    let discovered = [];
    let searchedPaths = {};
    let paths = await this._getPaths();

    winston.verbose(`discovering plugins..`, { label: 'pluginmanager'});

    for (const path of paths) {
      if (searchedPaths[path]) 
        continue;

      searchedPaths[path] = true;

      let plugins = await this._discoverPath(path);

      if (plugins.length === 0)
        continue;

      for(const plugin of plugins) {
        winston.verbose(`discovered plugin: ${plugin.name}..`, { label: 'pluginmanager'});
        discovered.push(plugin);
      }
    }

    return discovered;
  }

  _discoverPath = async (dir) => {
    try {
      winston.verbose(`checking ${dir} for plugins..`, { label: 'pluginmanager'});

      let plugins = [];

      // check the directory.
      if (!await fs.exists(dir))
        return plugins;

      // read subdirectories.
      let names = await fs.readdir(dir);

      // if this is a module directory with package.json, just skip it.
      if (await fs.exists(path.join(dir, 'package.json')))
        names = [""];

      for (const name of names) {
        let pluginPath = path.join(dir, name);
        var json = await this._checkPlugin(pluginPath);
        if (!json) continue;

        var plugin = new Plugin(name, pluginPath)
        plugins.push(plugin);
      }

      return plugins;
    } catch (err) {
      if (err.errno !== -4058) winston.error(err); // -4058 is just path not found error.
    }
  }

  _checkPlugin = async (dir) => {
    let stat = await fs.stat(dir);
    if (!stat.isDirectory()) return; // make sure it's a directory.

    let json = await Plugin.getJson(dir); // try loading the package.json of the plugin.
    return json;
  }

  _getPaths = async () => {
    let paths = [];

    paths = paths.concat(require.main.paths); // add require paths.
    paths.push(path.join(globalDirectories.npm.packages));

    return paths;
  }
}