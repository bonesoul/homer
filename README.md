<h1 align="center">homer</h1>

<div align="center">
<img src='https://github.com/bonesoul/homer/blob/develop/assets/images/homer/homer-lazy-1.gif?raw=true'/>
<br/><strong>The complete home automation for Homer Simpson.</strong>
</div>

<br />

<div align="center">
   <a href='https://github.com/bonesoul/homer/actions'>
     <img src='https://github.com/bonesoul/homer/workflows/build/badge.svg'/>
   </a>
</div>

<div align="center">
  <sub>Built with ❤︎ by <a href="https://github.com/bonesoul">Hüseyin Uslu</a>.</sub>
</div>

## what is homer?

homer is code-first home & office automation system allowing complex interactions between accessories.

## why?

as current so called "home automation" systems are actually just "home information" system, I needed a real "home automation" system where IoT stuff can interract which each other and respond to stuff happening at your home/office. Even more homer allows you to develop complex virtual orchestrators that can command stuff together. 

The problem is that with current config-first solutions, it's hard to wire off which the main reason I decided to develop a solution with code-first (programmatic) aproach.

<div align="center">
<img src='https://github.com/bonesoul/homer/blob/develop/assets/images/homer/homer-lazy-3.gif?raw=true'/>
<br/><strong>homer, orchestrating the office.</strong>
</div>

## sample scenarios

### turn on/off lights based on plex's status

a very basic scenario that anyone can expect from an so-called home-automation system right? with homer it's just a piece of cake;

```

let orchestrate = async () => {
    // get hue bridge.
    let huePlatform = this._platformRepository.active['Hue.Hue'];

    // wait for hue bridge to expose the bulbs.
    let lights = await getLights(huePlatform);

    // start listening for plex events.
    this._accessoryRepository.active['Plex.Plex'].getService(Service.OccupancySensor).getCharacteristic(Characteristic.OccupancyDetected).on('change', (data) => {
        for(const entry of lights) { // loop through all lights.
          entry.lightService.getCharacteristic(Characteristic.On).setValue(!data.newValue); // if plex started streaming close them, otherwise re-open them.
        }
    });
  }
  
let getLights = async (hue) => {
    return new Promise((resolve, reject) => {
      try {
        let lights = [];
        hue.accessories((accessories) => {
          for(const entry of accessories) {
            if (entry.constructor.name === 'HueAccessory' && entry.lightService) {
              lights.push(entry);
            }
          }
          return resolve(lights);
        });
      } catch (err) {
        return reject(err);
      }
    });
  }  
  
await orchestrate(); // let the magic happen!
```

here you can see it happening; [youtube](https://www.youtube.com/watch?v=ig6Ax14W_Dg)

<img src='https://media.giphy.com/media/TgaeFyjosJ3GeLzyR5/giphy.gif'/>

## status

still in early development stage.

- initially working on homekit support.
- will soon be able to pair with homekit.

## voice control support?

initially will have support for apple homekit, have plans for alexa & goole asistant support too.

## platforms

homer can run on a wide range of platforms including x86, x64, ARM32 and ARM64 - including Raspberry Pi or variants. Check [dotnet core runtime](https://github.com/dotnet/runtime/blob/master/src/libraries/pkg/Microsoft.NETCore.Platforms/runtime.json) too see all available runtimes.

## operating systems

- Windows
- macOS
- Linux (Redhat, Fedora, Debian, Ubuntu, Mint, openSUSE, SLES, Alphine, Rasbian..)

## requirements

```
dotnet core 3.1
```

## building

```
git clone https://github.com/bonesoul/homer
Windows: .\build.cmd
Linux: ./build.sh
MacOS: ./build.sh
```

## tests

```
dotnet test
``` 

<div align="center">
<img src='https://github.com/bonesoul/homer/blob/develop/assets/images/homer/homer-lazy-2.gif?raw=true'/>
</div>
