# Installing on Windows
## Requirements
* Gnomoria 1.0 
* * Steam version: https://store.steampowered.com/app/224500/Gnomoria/
* * GoG version: https://www.gog.com/game/gnomoria
## <a name="first_step"></a>First Step: Getting Gnoll
* Get to the release page and get the latest Installer release, for example: [Gnoll Latest Release](https://github.com/Nefaro/gnoll/releases/latest)
* Download the package
* Extract the package to your selected location

## <a name="second_step"></a>Second Step: Running the Installer
* Once you have the package extracted, run the "InstallerGUI.exe". This will open a window akin to this: ![Gnoll Installer](https://raw.githubusercontent.com/wiki/Nefaro/gnoll/images/Installer.png)
* Click "Browse ..." and select the directory where your "Gnomoria.exe" is located. 
* If all goes well, then the Installer will identify your game and will show you the latest patch available: 
* ![Installer with game exe](https://raw.githubusercontent.com/wiki/Nefaro/gnoll/images/Installer_game_exe.png)
* If everything goes according to plan, you have now 2 options:
* You can either mod the original Gnomoria.exe itself. 
* * This is useful with Steam, as this allows you to run the modded game via Steam (as usually) and include workshop mods in your games
* Or you can build a stand-alone exe
* * This allows you to run the Gnoll modded game separately from the unmodded, original game .exe. No workshop mods though, game needs to be started via Steam for that.
* If patching is successful, the Installer shows you the appropriate information: 
* ![Installer with successful modding](https://raw.githubusercontent.com/wiki/Nefaro/gnoll/images/Installer_successful_modding.png)
* You will find the stand-alone exe within your Gnomoria directory

## Third Step: Installing the mods
* If either of the modding options is used (meaning a standalone .exe exists or the original Gnomoria.exe is modified), the "Copy Mods" button will be enabled.
* Clicking on the "Copy Mods" button will copy the included mods into the right location.
* If there is already an existing mods folder, then this will be backed up.
* ![Gnoll Mods directory](https://raw.githubusercontent.com/wiki/Nefaro/gnoll/images/gnoll_mods_directory.png)

## Forth Step: Play the game
* Depending how you modded your game, run the game via Steam or run the stand-alone executable
* From within the game you should now see "Gnoll Mod Loader" menu option:
* ![Ingame Gnoll Menu](https://raw.githubusercontent.com/wiki/Nefaro/gnoll/images/ingame_gnoll_menu.png)

# Uninstall / Cleanup Gnoll
* The Installer also includes "Uninstall" button
* Using this action will remove the Gnoll Mods folder, will remove the ModLoader and uninstall / unpatch the executable. 
* After using the uninstall option, nothing of Gnoll should be left. 

# Installing on not-Windows
Ideally I would really love to support other platforms as well, alas I do not have the knowledge and/or the platform. I don't even know, if anyone has Gnomoria running on those other platforms. Anyway, if you have any constructive insight or even want to help with other platforms, I'm sure this will only make things better.

## Linux: Steam version
Surprisingly, installer works out of the box using protontricks to run the installer exe inside the Gnomoria prefix. 
Tested on Arch Linux using [GE-Proton7-14](https://github.com/GloriousEggroll/proton-ge-custom/releases/tag/GE-Proton7-14)
* Follow [First Step](#first_step) to get Gnoll
* Set up Steam to run Gnomoria via Proton instead of native and run the game at least once so the prefix gets created
* Use [protontricks](https://github.com/Matoking/protontricks#usage) to run the installer inside Proton/Wine prefix
* Mod the Gnomoria.exe following [next steps](#second_step) provided for Windows install
