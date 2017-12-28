# Prerequisites:

  - Gnomoria v1.0
  - Python 3.2+
  - .NET SDK 4.0 or newer

# Usage examples

```
./gnomodkit.py sdk
```

```
./gnomodkit.py mod:ExampleHelloWorld
```

```
./gnomodkit.py sdk mod:ExampleHelloWorld run
```

# How this thing works

## 1. Building SDK

- check retail Gnomoria.exe
- deobfuscate to make all symbol names valid identifiers; Gnomoria.exe -> GnomoriaGame.exe
- disassemble GnomoriaGame.exe -> GnomoriaGame.il
- make all fields & methods public; GnomoriaGame.il -> GnomoriaSDK.il
- assemble GnomoriaSDK.il -> GnomoriaSDK.dll
- copy retail gnomorialib.dll into working directory

GnomoriaSDK.dll + gnomorialib.dll can be now used to target Gnomoria internal APIs.

## 2. Building ModLoader

- compile ModLoader.dll, copy to game dir
- patch Gnomoria with ModLoader hooks; GnomoriaSDK.il + ModLoader.patch -> GnoMod.il
- assemble GnoMod.il -> GnoMod.exe
