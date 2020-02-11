
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
const winston = require('winston');

module.exports = {
  Logger: Logger,
  _system: new Logger() // system logger, for internal use only
}

// global cache of logger instances by plugin name
var loggerCache = {};

/**
 * Logger class
 */

function Logger(prefix) {
  this.prefix = prefix;
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
  let func = winston.info;

  if (level == 'debug') {
    func = winston.debug;
  }
  else if (level == 'verbose') {
    func = winston.verbose;
  }
  else if (level == 'warn') {
    func = winston.warn;
  }
  else if (level == 'error') {
    func = winston.error;
  }

  // prepend prefix if applicable
  if (this.prefix)
    msg = `[${this.prefix}] ${msg}`;

  func(msg);
}

Logger.withPrefix = function(prefix) {

  if (!loggerCache[prefix]) {
    // create a class-like logger thing that acts as a function as well
    // as an instance of Logger.
    var logger = new Logger(prefix);
    var log = logger.info.bind(logger);
    log.debug = logger.debug;
    log.info = logger.info;
    log.warn = logger.warn;
    log.error = logger.error;
    log.log = logger.log;
    log.prefix = logger.prefix;
    loggerCache[prefix] = log;
  }

  return loggerCache[prefix];
}