# Gnoll: Gnomoria Mod SDK and Mod Loader

Fork and repackage of "gnomodkit" from: https://github.com/minexew/gnomodkit

## Prerequisites:

  - Gnomoria v1.0 (Steam: https://store.steampowered.com/app/224500/Gnomoria/, GoG: https://www.gog.com/game/gnomoria)
  - Python 3.2+ (https://www.python.org/downloads/) 
  - Windows and .NET SDK 4.0 or newer  
  - Internet (build script downloads helper tools)
  
  (Note: Not sure if there exists any Linux tooling for the given operations)

## Steam Workshop Mods

Gnoll works with steam workshop mods. 
Gnoll does not currently have any content mods, so game data and save games are untouched.

To get Gnoll working with Steam, first build the modloader and mods, as shown in the Usage section. After successfully building the loader and mods, you should have "GnoMod.exe" in your Gnomoria directory. Next:
* Backup your Gnomoria.exe (rename Gnomoria.exe -> Gnomoria.orig.exe. if something ever happens to your original .exe, don't worry, ask Steam to verify game files and it will re-download it). 
* Rename Gnoll modified exe (rename GnoMod.exe -> Gnomoria.exe)
* Run Gnomoria via Steam as usually

## Quick installation guide (Longer version in wiki [Installation](../../wiki/Installation) )
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
- assemble GnoMod.il -> GnoMod.exe

## 3. Building Mods manually
- compile mod .dll
- copy the resulting .dll into "/Gnoll Mods/enabled" under Gnomoria directory 
- all .dlls in "Gnoll Mods/enabled" directory will be loaded by the Mod Loader