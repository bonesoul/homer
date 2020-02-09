// based on: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

// originally from: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

const fs = require('fs');
const path = require('path');
const plist = require('simple-plist');

var plistPath = './data/default.metadata.plist';
var metadata = plist.readFileSync(plistPath);

var characteristics = {};

for (var index in metadata.Characteristics) {
  var characteristic = metadata.Characteristics[index];
  var classyName = characteristic.Name.replace(/[\s\-]/g, ""); // "Target Door State" -> "TargetDoorState"
  classyName = classyName.replace(/[.]/g, "_"); // "PM2.5" -> "PM2_5"

  characteristics[characteristic.UUID] = classyName;

  var outputPath = path.join(__dirname, '..', '..', 'src', 'platforms', 'homekit', 'Characteristics',  'Definitions', `${classyName}Characteristic.cs`);
  var output = fs.createWriteStream(outputPath);

  output.write(`#region license
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
#endregion\n\n`);

output.write(`using System.Collections.Generic;\n\n`);
output.write(`namespace Homer.Platform.HomeKit.Characteristics.Definitions\n{\n`);
  output.write(`    public class ${classyName}Characteristic: Characteristic\n    {\n`);

  if (characteristic.Constraints && characteristic.Constraints.ValidValues) {
    // this characteristic can only have one of a defined set of values (like an enum). Define the values as static members of our subclass.
    output.write(`        // possible values:\n`);

    for (var value in characteristic.Constraints.ValidValues) {
      var name = characteristic.Constraints.ValidValues[value];

      var constName = name.toUpperCase().replace(/[^\w]+/g, '_');
      if ((/^[1-9]/).test(constName)) constName = "_" + constName; // variables can't start with a number

      output.write(`        public static int ${constName} = ${value};\n`);
    }
    output.write('\n');
  }

  output.write(`        public ${classyName}Characteristic(): base(\n`);
  output.write(`            uuid: "${characteristic.UUID}",\n`);
  output.write(`            displayName: "${characteristic.Name}",\n`);

  output.write(`            format: CharacteristicFormat.${getCharacteristicFormat(characteristic.Format)},\n`);

  if (characteristic.Unit)
    output.write(`            unit: CharacteristicUnit.${getCharacteristicUnit(characteristic.Unit)},\n`);

  // apply any basic constraints if present
  if (characteristic.Constraints && typeof characteristic.Constraints.MaximumValue !== 'undefined')
    output.write(`            maxValue: ${characteristic.Constraints.MaximumValue},\n`);

  if (characteristic.Constraints && typeof characteristic.Constraints.MinimumValue !== 'undefined')
    output.write(`            minValue: ${characteristic.Constraints.MinimumValue},\n`);

  if (characteristic.Constraints && typeof characteristic.Constraints.StepValue !== 'undefined')
    output.write(`            minStep: ${characteristic.Constraints.StepValue},\n`);

  if (characteristic.Constraints && characteristic.Constraints.ValidValues) {
    output.write(`            validValues: new List<int>\n            {\n`);
    for (var value in characteristic.Constraints.ValidValues) {
      output.write(`                ${value},\n`);
    }
    output.write(`            },\n`);
  }

  output.write(`            permissions: new List<CharacteristicPermission>\n            {\n`);
  for (var i in characteristic.Properties) {
    var perms = getCharacteristicPermsKey(characteristic.Properties[i]);
    if (perms) {
        output.write(`                CharacteristicPermission.${getCharacteristicPermsKey(characteristic.Properties[i])},\n`);
    }
  }
  output.write(`            })\n`);

  output.write(`        {\n        }\n    }\n}\n`);
  output.end()
}

function capitalize(s) {
  if (typeof s !== 'string') return ''
  return s.charAt(0).toUpperCase() + s.slice(1)
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