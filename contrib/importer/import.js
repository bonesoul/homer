// based on: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

// originally from: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

const fs = require('fs');
const path = require('path');
const nunjucks = require('nunjucks');
const plist = require('simple-plist');

var plistPath = './data/default.metadata.plist';
var metadata = plist.readFileSync(plistPath);

var characteristics = []; // index characteristics for quick access while building services.

readCharacteristics(metadata);
readServices(metadata);

function readCharacteristics(metadata) {
  for (var index in metadata.Characteristics) {
    var characteristic = metadata.Characteristics[index];
    var classyName = characteristic.Name.replace(/[\s\-]/g, ""); // "Target Door State" -> "TargetDoorState"
    classyName = classyName.replace(/[.]/g, "_"); // "PM2.5" -> "PM2_5"
    characteristics[characteristic.UUID] = classyName; // build characteristics list for later reference to use in services.

    var outputPath = path.join(__dirname, '..', '..', 'src', 'platforms', 'homekit', 'Characteristics',  'Definitions', `${classyName}Characteristic.cs`);
    var output = fs.createWriteStream(outputPath);

    var res = nunjucks.render('characteristic.html', {
      classyName: classyName,
      characteristic: characteristic,
      capitalize: capitalize,
      getCharacteristicUnit: getCharacteristicUnit,
      getCharacteristicFormat: getCharacteristicFormat,
      getCharacteristicPermsKey: getCharacteristicPermsKey,
      gotMaximumValue: gotMaximumValue,
      gotMinimumValue: gotMinimumValue,
      gotStepValue: gotStepValue,
    });

    output.write(res);
    output.end();
  }
}

function readServices(metadata) {
  for (var index in metadata.Services) {
    var service = metadata.Services[index];
    var classyName = service.Name.replace(/[\s\-]/g, "");

    var outputPath = path.join(__dirname, '..', '..', 'src', 'platforms', 'homekit', 'Services',  'Definitions', `${classyName}Service.cs`);
    var output = fs.createWriteStream(outputPath);

    var res = nunjucks.render('service.html', {
      classyName: classyName,
      service: service,
      characteristics: characteristics,
      capitalize: capitalize,
    });

    output.write(res);
    output.end();
  }
}

function capitalize(s) {
  if (typeof s !== 'string') return ''
  return s.charAt(0).toUpperCase() + s.slice(1)
}

function gotMaximumValue(characteristic) {
  return characteristic.Constraints && typeof characteristic.Constraints.MaximumValue !== 'undefined';
}

function gotMinimumValue(characteristic) {
  return characteristic.Constraints && typeof characteristic.Constraints.MinimumValue !== 'undefined';
}

function gotStepValue(characteristic) {
  return characteristic.Constraints && typeof characteristic.Constraints.StepValue !== 'undefined'
}

function getCharacteristicUnit(unit) {
  unit = capitalize(unit);
  if (unit == 'Arcdegrees') unit = 'ArcDegree'
  return unit;
}

function getCharacteristicFormat(format) {
  format = capitalize(format);
  if (format == 'Int32') format = 'Int'
  return format;
}

function getCharacteristicPermsKey(perm) {
  switch (perm) {
    case "read": return "PairedRead";
    case "write": return "PairedWrite";
    case "cnotify": return "Events";
    case "uncnotify": return undefined;
    default: throw new Error("Unknown characteristic permission '" + perm + "'");
  }
}