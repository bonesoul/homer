
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

const fs = require('fs');
const path = require('path');
const config = require('config');
const winston = require('winston');
const util = require('util');
const user = require('lib/user');
const CustomLogger = require('lib/logger/custom');
const { format } = require('winston');
const { combine, colorize, timestamp, ms, label, splat, printf } = format;

module.exports.initialize = async () => {
  try {
    // make sure log path exists.
    const logPath = user.logPath();
    fs.existsSync(logPath) || fs.mkdirSync(logPath, { recursive: true });

    // setup the console log if enabled.
    if (config.logging.master.console.enabled) {
      winston.add(new winston.transports.Console({
        level: config.logging.master.console.level,
        format: combine(
          timestamp({format: timeStamp}),
          ms(),
          label({ label: '' }),
          colorize({ level: true }),
          splat(),
          logFormat,
        )
      }));
    }

    // setup the server log if enabled.
    if (config.logging.master.file.enabled) {
      winston.add(new winston.transports.File({
        level: config.logging.master.file.level,
        format: combine(
          timestamp({format: timeStamp}),
          ms(),
          label({ label: '' }),
          splat(),
          logFormat,
        ),
        filename: path.join(logPath, 'homer.log')
      }));
    }
  } catch (err) {
    throw new Error(`Error initiliazing logger - ${err}.`);
  }
};

module.exports.customLogger = (dir, type, name) => {
  try {
    const logger = createCustomLogger(dir, type, name); // the custom winston logger.
    const customLoger = new CustomLogger(logger);

    var log = customLoger.info.bind(customLoger);
    log.debug = customLoger.debug;
    log.info = customLoger.info;
    log.warn = customLoger.warn;
    log.error = customLoger.error;
    log.log = customLoger.log;
    log.logger = customLoger.logger;
    console.dir(log.logger)
    return log;
  } catch (err) {
    throw new Error(`error initiliazing custom logger - ${err}.`);
  }
};

const createCustomLogger = (dir, type, name) => {
  // setup the logging directory and ensure it exists.
  dir = dir.toLowerCase();
  type = type.toLowerCase();
  name = name.toLowerCase();
  const logPath = path.join(user.logPath(), dir, type);
  fs.existsSync(logPath) || fs.mkdirSync(logPath, { recursive: true }); // eslint-disable-line security/detect-non-literal-fs-filename

  const logger = winston.createLogger({});

  if (config.logging.plugin.console.enabled) {
    logger.add(new winston.transports.Console({
      level: config.logging.master.console.level,
      format: combine(
        timestamp({format: timeStamp}),
        ms(),
        label({ label: `${type}:${name}` }),
        colorize({ level: true }),
        splat(),
        logFormat,
      )
    }));
  }

  if (config.logging.plugin.file.enabled) {
    logger.add(new winston.transports.File({
      level: config.logging.master.file.level,
      format: combine(
        timestamp({format: timeStamp}),
        ms(),
        label({ label: `${type}:${name}` }),
        splat(),
        logFormat,
      ),
      filename: path.join(logPath, `${name}.log`)
    }));
  }

  return logger;
};

const timeStamp = () => {
  var date = new Date();
  return `${date.getDate()}/${(date.getMonth() + 1)} ${date.toTimeString().substr(0, 8)}`;
}

const logFormat = printf((info) => {
  let context = info.label ? ` [${info.label}] ` : ''
  return `${info.timestamp} [${info.ms}] - ${info.level}:${context}${info.message}`;
});

