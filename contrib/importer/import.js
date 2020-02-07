// based on: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

// originally from: https://github.com/KhaosT/HAP-NodeJS/blob/master/src/lib/gen/importAsClasses.ts

const fs = require('fs');
const path = require('path');
const plist = require('simple-plist');

var plistPath = './data/default.metadata.plist';
var metadata = plist.readFileSync(plistPath);

var outputPath = path.join(__dirname, 'HomeKitTypes.txt');
var output = fs.createWriteStream(outputPath);

var characteristics = {};

for (var index in metadata.Characteristics) {
  var characteristic = metadata.Characteristics[index];
  var classyName = characteristic.Name.replace(/[\s\-]/g, ""); // "Target Door State" -> "TargetDoorState"
  classyName = classyName.replace(/[.]/g, "_"); // "PM2.5" -> "PM2_5"

  characteristics[characteristic.UUID] = classyName;

  output.write(`using System.Collections.Generic;\n\n`);
  output.write(`namespace Homer.Platform.HomeKit.Characteristics.Definitions\n{\n`);
  output.write(`    public class ${classyName}Characteristic : Characteristic\n    {\n`);

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

  output.write(`        public ${classyName}Characteristic() : base(\n`);
  output.write(`            uuid: "${characteristic.UUID}",\n`);
  output.write(`            displayName: "${characteristic.Name}",\n`);

  output.write(`            format: CharacteristicFormat.${capitalize(characteristic.Format)},\n`);

  output.write(`            permissions: new List<CharacteristicPermission>\n            {\n`);
  for (var i in characteristic.Properties) {
    var perms = getCharacteristicPermsKey(characteristic.Properties[i]);
    if (perms) {
        output.write(`                CharacteristicPermission.${getCharacteristicPermsKey(characteristic.Properties[i])},\n`);
    }
  }
  output.write(`            },\n`);

  if (characteristic.Unit)
    output.write(`            unit: CharacteristicUnit.${capitalize(characteristic.Unit)},\n`);

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
    output.write(`            }\n`);
  }

  output.write(`            )\n`);
  output.write(`        {\n        }\n    }\n}\n`);

  output.write(`\n\n`);
}

function capitalize(s) {
  if (typeof s !== 'string') return ''
  return s.charAt(0).toUpperCase() + s.slice(1)
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