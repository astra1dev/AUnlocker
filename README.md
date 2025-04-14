<p align="center">
  <img src="./icon.png">
</p>

<p align="center">
  <a href="https://www.gnu.org/licenses/gpl-3.0.html">
    <img src="https://img.shields.io/badge/license-GPL-yellow.svg?style=plastic&logo=GNU&label=License">
  </a>
  <a href="https://github.com/astra1dev/AUnlocker/actions/workflows/main.yml">
    <img src="https://github.com/astra1dev/AUnlocker/actions/workflows/main.yml/badge.svg?event=push&style=plastic">
  </a>
  <a href="../../releases">
    <img src="https://img.shields.io/github/downloads/astra1dev/AUnlocker/total.svg?style=plastic&color=red">
  </a>
  <a href="../../releases/latest">
    <img src="https://img.shields.io/github/downloads/astra1dev/AUnlocker/latest/total?style=plastic">
  </a>
</p>

<p align="center">
<b>🔓Unlock Among Us: cosmetics, account, chat and more!🎉</b>

# 🎉 Features
- 🎭 Unlock Cosmetics[*](/README.md#️-disclaimer) (Hats, Visors, Skins, Pets, Nameplates, Cosmicubes, Bundles)
- 🌐 Account Features
  - Removes the following restrictions that apply to guest accounts:
    - Not being able to set a custom name
    - Quick Chat Only
    - Not being able to use the friend list
  - Removes the following restrictions that apply to minor accounts:
    - Not being able to play online
  - Removes the time penalty after disconnecting from too many lobbies
- 💬 Chat Features
  - Use `Ctrl + C` and `Ctrl + V` to copy-paste chat messages
  - Be able to send URLs and Email addresses
  - Increase the character limit from 100 to 120
- ✨ Other Features
  - Remove the 60 FPS cap 
  - Prevent the game from collecting analytics and sending them to Innersloth
  - Be able to activate the April Fools Mode (Long Boi and Horse Mode)
  - Show more information when finding a game: host name, lobby code, host platform, and lobby age
- 💣 Unsafe
  - Be able to send any character in chat
  - No character limit in chat
  - No 3s cooldown between chat messages

> [!NOTE]
> Features can be individually enabled & disabled by editing the file `[YOUR_AMONG_US_FOLDER]/BepInEx/config/AUnlocker.cfg`
>
> `💣 Unsafe` Features can get you kicked by the anti-cheat. Use them with caution.

# 🔥 Releases
Before you download anything, make sure your platform is supported:
- ✅ PC / Desktop (Steam, itch.io, Epic Games, Microsoft Store)
- ❌ Mobile (Google Play & App Store)
- ❌ Console (Playstation, Nintendo Switch, Xbox)

The table below lists the most recent AUnlocker release for each Among Us version. Release notes can be found below each new [release](../../releases).

|    Among Us Version    |          AUnlocker Version          |
|:----------------------:|:-----------------------------------:|
| `16.0.2` (`2025.3.31`) | [v1.1.8](../../releases/tag/v1.1.8) |
| `16.0.0` (`2025.3.25`) | [v1.1.8](../../releases/tag/v1.1.8) |
|      `2024.11.26`      | [v1.1.7](../../releases/tag/v1.1.7) |
|      `2024.10.29`      | [v1.1.7](../../releases/tag/v1.1.7) |
|       `2024.9.4`       | [v1.1.7](../../releases/tag/v1.1.7) |
|      `2024.8.13`       | [v1.1.6](../../releases/tag/v1.1.6) |
|      `2024.6.18`       | [v1.1.5](../../releases/tag/v1.1.5) |
|      `2024.3.05`       | [v1.1.4](../../releases/tag/v1.1.4) |
|      `2023.11.28`      | [v1.1.3](../../releases/tag/v1.1.3) |
|      `2023.10.24`      | [v1.1.0](../../releases/tag/v1.1.0) |

# 💾 Installation
## 🪟 Windows
- [Download](../../releases/latest) either `AUnlocker_v*_Steam_Epic_Itch.zip` or `AUnlocker_v*_MicrosoftStore.zip` depending on your edition of Among Us.
- Extract the contents of the zip into your Among Us folder. You can find your Among Us folder like this:
  - **Steam:** Right-click Among Us in your library → `Manage` → `Browse local files`
  - **Epic Games:** Right-click Among Us in your library → `Manage` → click the small folder icon next to `Installation`
  - **Itch.io:** Open the Itch.io app → Right-click Among Us in your library → `Manage` → `Open folder in Explorer`.
  - **Microsoft Store:** Check [this support article](https://answers.microsoft.com/en-us/xbox/forum/all/where-can-i-find-the-gamefiles-of-a-game/5cb9a0c3-7948-4316-abc5-f27d1767b932) on how to find and access your Among Us folder.
- Your game folder should look like this after installation:
<img src="https://github.com/astra1dev/AUnlocker/assets/90265231/14226f03-a003-4efc-b27b-6df53fb394d6" width=410 height=240>

- Launch Among Us. The first launch will take **MUCH** longer, so don't worry if you have to wait a few minutes.

## 🐧 Linux
- Make sure you are running Among Us under Proton. On Steam you can check this by right-clicking Among Us in your library → `Properties` → `Compatibility` → `Force the use of a specific Steam Play compatibility tool`. You might want to test different Proton versions if you're having issues launching the game. 
- Check out [this guide](https://docs.bepinex.dev/articles/advanced/proton_wine.html) and [this guide](https://docs.bepinex.dev/master/articles/advanced/steam_interop.html) to get BepInEx (the framework AUnlocker is built upon) working. Then follow the steps for Windows.
- If you are experiencing crashes or errors like `Unable to execute IL2CPP chainloader`, set your launch arguments (right-click Among Us in your library → `Properties` → `General` → `Launch Options`) to `PROTON_NO_ESYNC=1 PROTON_USE_WINED3D=1 %command%` 

<hr>

<b>👾 If you are already using other mods or already have BepInEx installed:</b>
- You should see a folder called `BepInEx` inside your Among Us folder. 
- Download `AUnlocker_v*.dll` from the [latest release](../../releases/latest), put it into `BepInEx/plugins` and launch Among Us.

<b>👷‍♂️ If you don't want to download the pre-compiled DLL, you can build AUnlocker from source by following these steps:</b>
- Make sure you have the [Microsoft .NET SDK](https://dotnet.microsoft.com/en-us/download) and [git](https://git-scm.com/downloads) installed.
- Download the necessary files with `git clone https://github.com/astra1dev/AUnlocker`
- Run the command `dotnet build` from the AUnlocker folder (where `AUnlocker.sln` is located)
- The compiled DLL will be located here: `src/bin/Debug/net6.0/AUnlocker.dll`. Put it into `BepInEx/plugins` and launch Among Us.

# 👨‍💻 Contributing
General contribution:
- For bug reports and feature requests, [open a new issue](/issues/new)
- If you want to add a new feature or edit / improve existing code, fork this repo and create a pull request with your changes.  ([how?](https://docs.github.com/en/get-started/exploring-projects-on-github/contributing-to-a-project))

Getting started modding Among Us:
- Learn [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) and [Unity](https://unity.com), as well as [BepInEx](https://github.com/BepInEx/BepInEx) and [HarmonyX](https://github.com/BepInEx/HarmonyX)
- Read the [docs](https://docs.reactor.gg) for [Reactor](https://github.com/NuclearPowered/Reactor), a modding API for Among Us. \
  Join their [discord](https://reactor.gg/discord) for the latest `Assembly-CSharp.dll` files. Open them using [dnSpy](https://github.com/dnSpy/dnSpy) to view the game's decompiled client code.
- Take a look at [sus.wiki](https://github.com/roobscoob/among-us-protocol) to learn more about the Among Us network protocol

For more detailed contribution guidelines, read [CONTRIBUTING.md](/.github/CONTRIBUTING.md)

# ⚠️ Disclaimer
AUnlocker does not unlock all cosmetics **permanently**, so it does **not** add them to your account. This is because your progress is stored on the Innersloth servers. If you uninstall this mod, the cosmetics will be locked again. The cosmetics you have already unlocked, e.g. through buying a cosmicube, are **untouched** by this mod.

This mod is not affiliated with Among Us or Innersloth LLC, and the content contained therein is not endorsed or otherwise sponsored by Innersloth LLC. Portions of the materials contained herein are property of Innersloth LLC. © Innersloth LLC.
