# Installer logic

## Game directory detection

### Steam

TBD

### GOG

`HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\GOG.com\Games\1460101597`

## Scan logic

- look for gnoll-version.json (aka InstallDB) in game dir
  - if not found
    - assume no standalone installs
    - check if game exe corresponds to known modded versions (TODO)
      - if it does, update InstallDB
      - otherwise, assume exe unmodded
  - if found, it will tell us if exe is modded (and how), and which standalone installs there are

- if exe unmodded, get its md5
  - otherwise read from InstallDB
- check that Gnomoria.exe version known

- offer action InstallModKit if game unmodded and recognized (patch available)
- offer action UpgradeModKit if game modded to lesser version and a backup is available and we have new patch
- offer action InstallStandalone if current version not present and patch is available
- offer action Launch for vanilla or modded exe + any standalone installs
- offer action UninstallModKit for modded exe
- offer action UninstallStandalone for any standalone installs

## Payloads

- Under InstallerCore/Payloads
- File name format $"G{buildNumber}_{gameMd5.Substring(0, 8)}.xdelta"
  e.g. G1700_c9f6d4b9.xdelta

- The project ships with sample dummy payloads (build number `G0000`); these must be replaced (in Resources.resx) with proper patches to produce a release
