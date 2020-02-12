
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
const moment = require('moment');
const user = require('lib/user');
const { format, info, transports } = require('winston');
const { combine, cli, colorize, padLevels, timestamp, ms, label, splat, printf } = format;
require('moment-duration-format')(moment);
const hirestime = require('hirestime');

module.exports.initialize = async () => {
  try {
    // setup the logging directory and ensure it exists.
    const logPath = path.join(__dirname, '../../../logs');
    fs.existsSync(logPath) || fs.mkdirSync(logPath); // eslint-disable-line security/detect-non-literal-fs-filename

    const masterFormat = printf((info) => {
      return `${info.timestamp} [${info.ms}] - ${info.level}: [${info.label}] ${info.message}`;
    });

    const timeStamp = () => {
      var date = new Date();
      return `${date.getDate()}/${(date.getMonth() + 1)} ${date.toTimeString().substr(0, 8)}`;
    }

    winston.configure({
      transports: [
        new winston.transports.Console({
          level: 'verbose',
          format: combine(
            timestamp({format: timeStamp}),
            ms(),
            label({ label: 'right meow!' }),
            colorize({ level: true }),
            splat(),
            masterFormat,
          )
        }),
        new winston.transports.File({
          level: 'verbose',
          format: combine(
            timestamp({format: timeStamp}),
            ms(),
            label({ label: 'right meow!' }),
            splat(),
            masterFormat,
          ),
          filename: user.logPath()
        })
      ]
    });
  } catch (err) {
    throw new Error(`Error initiliazing logger - ${err}.`);
  }
};

module.exports.taskLogger = taskName => {
  try {
    const logger = createCustomLogger(taskName); // the custom winston logger.
    let profiler = null; // the profiler.

    return {
      log: logger,
      started: function () {
        // commit the start notice.
        logger.info('---------------------------------------------');
        logger.info(`starting: ${moment.utc()}`);
        logger.info('---------------------------------------------');

        // start profiling.
        profiler = hirestime();
      },
      finished: function (stats, fails = null) {
        // end profiling.
        const duration = profiler();
        const elapsed = moment.duration(duration).format();

        // commit the end notice.
        logger.info('---------------------------------------------');
        logger.info(`completed: in ${elapsed}..`, stats);
        logger.info('---------------------------------------------');

        // summarize fails if needed.
        if (fails && fails.length > 0) {
          logger.info('failed:');
          for (const entry of fails) {
            logger.info(` - ${entry}`);
          }
        }

        return elapsed; // also return the elapsed time.
      }
    };
  } catch (err) {
    throw new Error(`Error initiliazing task logger - ${err}.`);
  }
};

const createCustomLogger = taskName => {
  // setup the logging directory and ensure it exists.
  const logPath = path.join(__dirname, '../../../logs/tasks');
  fs.existsSync(logPath) || fs.mkdirSync(logPath); // eslint-disable-line security/detect-non-literal-fs-filename

  const transports = [];
  if (config.logging.task.enabled)
    transports.push(new winston.transports.File({
      name: `${taskName}`,
      filename: path.join(logPath, `/${taskName}.log`),
      level: config.logging.task.level,
      colorize: false,
      json: false
    }));

  const logger = new winston.Logger({
    transports: transports,
    exitOnError: false
  });

  return logger;
};
