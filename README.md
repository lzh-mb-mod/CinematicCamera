# Cinematic Camera

A mod that allows you to adjust camera to make cinematic video.

## Features
- Adjust FOV, depth of field, moving speed of camera in [`RTS Camera`](https://www.nexusmods.com/mountandblade2bannerlord/mods/355).

- Optionally set player character invulnerable to avoid it being killed accidentally.

- Configuration saving. The configuration is saved in directory `(user directory)\Documents\Mount and Blade II Bannerlord\Configs\RTSCamera\`.
  
  The config file is saved in file `CinematicCameraConfig.xml`.

  You can modify them manually, but if you edit it incorrectly or remove them, the configuration will be reset to default.

## Prerequisite
- [`RTS Camera`](https://www.nexusmods.com/mountandblade2bannerlord/mods/355).

## How to install
1. Please download and install the prerequisite mod [`RTS Camera`](https://www.nexusmods.com/mountandblade2bannerlord/mods/355) first.

2. Copy `Modules` folder into Bannerlord installation folder(For example `C:\Program Files\Steam\steamapps\common\Mount & Blade II Bannerlord - Beta`). It should be merged with `Modules` of the game. Or use Vortex to install it automatically.


## Loading sequence requirement
- `Cinematic Camera` should be loaded after `RTS Camera`.

## How to use
- Start the launcher and choose Single player mode. In `Mods` panel select `RTS Camera` and `Cinematic Camera`, then click `PLAY`.

  Note: Please make sure that `Cinematic Camera` is loaded after `RTS Camera`. For now the loading sequence in official launcher is **incorrect**.

  You can use launcher fix mod or use Vortex to sort them automatically.

- Then play the game as usual.

- After entering a mission (scene):

  - Press `O(letter)` (by default) and then click `Extension: Cinematic Camera` to open menu of this mod. You can access the options of this mod in it.

  - All the options will be applied when camera is in RTS mode.  Press `F10` by default to enable it. For details please read instructions in `RTS Camera`.

## Troubleshoot
- If the launcher can not start:

  - Uninstall all the third-party mods and reinstall them one by one to detect which one cause the launcher cannot start.

- If it shows "Unable to initialize Steam API":

  - Please start steam first, and make sure that Bannerlord is in your steam account.

- If the game crashed after starting:

  - Please make sure the loading sequence is correct.

  - Please uncheck the mod in launcher and wait for mod update.

    Optionally you can tell me the step to reproduce the crash.

## Contact with me
* Please mail to: lizhenhuan1019@qq.com
