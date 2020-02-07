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

As current so called "home automation" systems are actually just "home information" system, I needed a real "home automation" system where IoT stuff can interract which each other and respond to stuff happening at your home/office. Even more homer allows you to develop complex virtual orchestrators that can command stuff together.

<div align="center">
<img src='https://github.com/bonesoul/homer/blob/develop/assets/images/homer/homer-lazy-3.gif?raw=true'/>
<br/><strong>homer, orchestrating the office.</strong>
</div>

## status

still in early development stage.

- initially working on homekit support.
- will soon be able to pair with homekit.

## voice control support?

initially will have support for apple homekit, have plans for alexa & goole asistant support too.

## runtimes

homer can run on a wide range of platforms including x86, x64, ARM32 and ARM64 - including Raspberry Pi or variants. Check [dotnet core runtime](https://github.com/dotnet/runtime/blob/master/src/libraries/pkg/Microsoft.NETCore.Platforms/runtime.json) too see all available runtimes.

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
