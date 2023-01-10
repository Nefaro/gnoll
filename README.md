# Gnoll: Gnomoria Mod SDK and Mod Loader

Fork and repackage of "gnomodkit" from: https://github.com/minexew/gnomodkit

Gnoll is a package or packaging of a group of tools that allows building, loading and running .dll (C# source code level) mods in Gnomoria. Gnoll consist of 4 sets of components:
- Patches
- ModLoader
- Mods themselves
- Installer

**Patches** are the code changes needed for everything Gnoll to work within Gnomoria. We use manual crafted IL (https://en.wikipedia.org/wiki/Common_Intermediate_Language) patches as well as [Harmony](https://github.com/pardeike/Harmony) for newer patches. 

**ModLoader** is the component that is hooked into Gnomoria by the patches. It is responsible for being the "glue" between the game and the provided mods.

**Mods** are the components loaded by ModLoader and add additional functionality or, on some cases, fix or work around bugs.

**Installer** is the packaging that is released and is responsible for applying the patches, installing the modloader and the mods.

## Installing via Installer (the suggested way)
Take a look in here: [Installation via Installer](../../wiki/Installation-via-the-Installer)

**Note:** However, one should not be in the habbit of downloading random executables from the internet, as such you can always build the mods yourself.

## Prerequisites for Manual Install:

  - Gnomoria v1.0 (Steam: https://store.steampowered.com/app/224500/Gnomoria/, GoG: https://www.gog.com/game/gnomoria)
  - Python 3.2+ (https://www.python.org/downloads/) 
  - Windows
  - SKD / .NET Framework 4.8 Tools or newer (for ildasm.exe) 
  - .NET Framework 4.0 or newer (for ilasm.exe)
  - Compiler: MSBuild (14.0+) for using (gnomodkit/gnomkit) or Visual Studio 2015+ for building yourself
  - Internet (build script downloads helper tools)
  
  (Note: Not sure if there exists any Linux tooling for the given operations)

## Steam Workshop Mods

Gnoll works with steam workshop mods. 
Gnoll does not currently have any content mods, so game data and save games are untouched.

To get Gnoll working with Steam, first build the modloader and mods, as shown in the Usage section. After successfully building the loader and mods, you should have "GnoMod.exe" in your Gnomoria directory. Next:
* Backup your Gnomoria.exe (rename Gnomoria.exe -> Gnomoria.orig.exe. if something ever happens to your original .exe, don't worry, ask Steam to verify game files and it will re-download it). 
* Rename Gnoll modified exe (rename GnoMod.exe -> Gnomoria.exe)
* Run Gnomoria via Steam as usually

OR Run
```
py gnomodkit.py steam
```
which will do all that for you. To get back the original .exe
```
py gnomodkit.py unsteam
```

## Quick Manual installation guide (Longer version in wiki [Installation](../../wiki/Manual-Installation) )
Assumption: Python executable for Windows is named 'py'. Might be something else, modify commands accordingly.

Assumption: All prerequisites have been met/installed

Building SDK
```
py gnomodkit.py sdk
```
Building ModLoader
```
py gnomodkit.py modloader
```
Building a mod contained in this project (mod name = directory name under "Gnoll Mods")
```
py gnomodkit.py mod:ExampleHelloWorld
```
Building ALL mods contained in this project
```
py gnomodkit.py mod:all
```
Build all mods and run the modded game
```
py gnomodkit.py sdk mod:all run
```
Install modded .exe to run via Steam (to be able to use Steam workshop mods)
```
py gnomodkit.py sdk steam
```
The ultimate command. Run this and once it's finished, start the game via Steam as usual
```
py gnomodkit.py sdk mod:all steam
```

# Included mods

Check out the existing mods under "Gnoll Mods" directory

# Screenshots

For pictures visit this Steam discussion:
https://steamcommunity.com/app/224500/discussions/0/3042732434759234670/

# How this thing works

Note: This here explains how things work. The actual work should be done via gnomodkit.py script.

## 1. Building SDK

- check retail Gnomoria.exe
- deobfuscate to make all symbol names valid identifiers; Gnomoria.exe -> GnomoriaGame.exe
- disassemble GnomoriaGame.exe -> GnomoriaGame.il
- make all fields & methods public; GnomoriaGame.il -> GnomoriaSDK.il
- assemble GnomoriaSDK.il -> GnomoriaSDK.dll
- copy retail gnomorialib.dll into working directory

GnomoriaSDK.dll + gnomorialib.dll can be now used to target Gnomoria internal APIs.

## 2. Building ModLoader

- compile GnollModLoader.dll, copy to game dir
- patch Gnomoria with ModLoader hooks; GnomoriaSDK.il + GnollModLoader.patch -> GnoMod.il
- assemble GnoMod.il -> GnomoriaSDK-patched.dll
- assemble GnoMod.il -> GnoMod.exe

## 3. Building Mods manually
- compile mod .dll
- copy the resulting .dll into "/Gnoll Mods/enabled" under Gnomoria directory 
- all .dlls in "Gnoll Mods/enabled" directory will be loaded by the Mod Loader
