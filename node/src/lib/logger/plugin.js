
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

const util = require('util');

module.exports = {
  Logger: Logger
}

function Logger(logger) {
  this.logger = logger;
}

Logger.prototype.debug = function(msg) {
  this.log.apply(this, ['debug'].concat(Array.prototype.slice.call(arguments)));
}

Logger.prototype.info = function(msg) {
  this.log.apply(this, ['info'].concat(Array.prototype.slice.call(arguments)));
}

Logger.prototype.warn = function(msg) {
  this.log.apply(this, ['warn'].concat(Array.prototype.slice.call(arguments)));
}

Logger.prototype.error = function(msg) {
  this.log.apply(this, ['error'].concat(Array.prototype.slice.call(arguments)));
}

Logger.prototype.log = function(level, msg) {
  msg = util.format.apply(util, Array.prototype.slice.call(arguments, 1));
  let func = this.logger.info;

  if (level == 'debug')
    func = this.logger.debug;
  else if (level == 'warn')
    func = this.logger.warn;
  else if (level == 'error')
    func = this.logger.error;

  func(msg);
}