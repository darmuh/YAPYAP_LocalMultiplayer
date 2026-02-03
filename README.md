# LocalMultiplayer

**Play multiplayer locally with one Steam account.**

## Features

This mod allows you to connect multiple YAPYAP instances to the same multiplayer game using one Steam account.

## Usage

This mod requires you to create your own Photon cloud application. (This does not require any payment.)

### 1. Photon

> [!TIP]
> If you have previously set up the [Local Multiplayer mod for REPO](https://thunderstore.io/c/repo/p/Zehs/LocalMultiplayer/) you can re-use the same photon voice server for YAPYAP.

1. Go to the [Photon Engine](https://www.photonengine.com) website and sign in or create an account.
2. Navigate to the [Dashboard](https://dashboard.photonengine.com).
3. Create a new Photon cloud application and select `Voice` for the Photon SDK.
4. Choose any name you would like and click Create.

### 2. Config Settings

1. Open the config file. (See the Config Settings section on how to find the config file.)
2. Set `Voice App ID` to your Photon `Voice` application's App ID.

### 3. Steam Launch Options

> [!TIP]
> If you use the Gale mod manager, you can just set your launch mode to Direct in the settings and skip this step.

1. Go to Steam and right-click YAPYAP.
2. Click "Properties..."
3. In the General tab, you should see an input field called "LAUNCH OPTIONS"
4. Put this for your launch options:
```
--doorstop-enable true --doorstop-target "YOUR_PROFILE_LOCATION_HERE\BepInEx\core\BepInEx.Preloader.dll"
```

> [!IMPORTANT]
> The file path must lead to your profile's `BepInEx.Preloader.dll` file in the `BepInEx/core` folder.

### 4. Testing

1. Open the game.
2. Go to Options > Display.
3. Set your Screen Mode to Windowed. (for easier testing)
4. Click Play (select a save slot if prompted)
5. You should launch to a loading screen with a window on the left hand side allowing you to specify username, IP Address, Port and 3 buttons labeled Host, Server, and Client.
6. For the first game session, click ``Host``. You do not need to modify the username, IP address, or port information.
7. Open the game again. (You will have two YAPYAP instances open at this point.)
8. Click the Play button on the second instance (save slot selection should not matter)
9. In this loading screen, click ``Client`` as you will be joining the session of your host client.

## Config Settings

You must open your game at least once with the mod installed for the config file to get generated.

This mod uses a global setting file so you don't have to configure your settings for each modpack/profile you use and to prevent your Photon App ID from being transferred in your profile codes.

You can locate the setting file at this path:
```
%userprofile%\appdata\LocalLow\maisonbap\YAPYAP\LocalMultiplayer\AppIdVoice.txt
```

## Credits
ZehsTeam for [REPO-LocalMultiplayer](https://github.com/ZehsTeam/REPO-LocalMultiplayer) (Used as inspiration for voice solution as well as README content)