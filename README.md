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
  <a href="https://discord.gg/VXWgMKwXeQ">
    <img src="https://img.shields.io/badge/Join%20us%20on-Discord-blue?style=plastic&logo=discord" alt="Discord">
  </a>
</p>

<p align="center">
<b>🔓Unlock Among Us: cosmetics, account, chat and more!🎉</b>


# 🎉 Features

| Unlock Cosmetics | Account Patches | Chat Patches | Other Patches |
|------------------|-----------------|--------------|---------------|
| Hats             | Unlock free chat          | Copy-pasting allowed        | Unlock FPS
| Visors           | Unlock friend list        | URLs and Emails allowed     | Unlock April Fools Mode
| Skins            | Unlock custom name        | Special characters allowed  | NoTelemetry
| Pets             | Unlock online gameplay    | 
| Nameplates       | Remove "minor" status     | 
| Cosmicubes       | Remove disconnect penalty |
| Bundles          |                           |

These features can be individually enabled / disabled by editing the file `[YOUR_AMONG_US_FOLDER]/BepInEx/config/AUnlocker.cfg`

<hr>

# 🔥 Releases
Before you download anything, make sure your platform is supported:
- ✅ PC / Desktop (Steam, itch.io, Epic Games)
- ❌ Microsoft Store
- ❌ Mobile (Google Play & App Store)
- ❌ Consoles (Playstation, Nintendo Switch, XBox)


The table below lists the most recent AUnlocker release for each Among Us version. Patch notes can be seen under each new [release](https://github.com/astra1dev/AUnlocker/releases).

| Among Us Version | AUnlocker Version |
|:-:|:-:|
| `2024.8.13`        | v1.1.6 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.6) |
| `2024.6.18`        | v1.1.5 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.5) |
| `2024.3.05`        | v1.1.4 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.4) |
| `2023.11.28`       | v1.1.3 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.3) |
| `2023.11.28`       | v1.1.2 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.2) |
| `2023.11.28`       | v1.1.1 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.1) |
| `2023.10.24`       | v1.1.0 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.0) |


<hr>

# 💾 Installation
## Windows
- Download the latest `AUnlocker_v*.zip` file from [here](https://github.com/astra1dev/AUnlocker/releases). Make sure you are running the latest Among Us version.
- Extract the contents of the zip into your Among Us folder. You can find your Among Us folder like this:
  - **Steam:** Right-click Among Us in your library → click `manage` → click on `browse local files`
  - **Epic Games:** Right-click Among Us in your library → click `manage` → click on the little folder icon next to `Installation`
  - **Itch.io:** Open the Itch.io app → Right-click Among Us in your library → click `manage` → click `open folder in Explorer`.
- Your game folder should look like this after installation:
<img src="https://github.com/astra1dev/AUnlocker/assets/90265231/14226f03-a003-4efc-b27b-6df53fb394d6" width=410 height=240>

- Launch Among Us. The first launch will take **MUCH** longer, so don't worry if you have to wait a few minutes.

## 🐧 Linux
Check out [this guide](https://docs.bepinex.dev/articles/advanced/proton_wine.html) and [this guide](https://docs.bepinex.dev/master/articles/advanced/steam_interop.html) to get BepInEx (framework AUnlocker is built upon) working. Then follow the steps for Windows.

<hr>

<b>If you already installed one or more Among Us mods in the past:</b>
- You should see a folder called `BepInEx` inside your Among Us folder. 
- Download `AUnlocker_v*.dll` from the [latest release](https://github.com/astra1dev/AUnlocker/releases), place it into `BepInEx/plugins` and launch Among Us.

<b>If you don't want to download the pre-compiled DLL, you can build AUnlocker from source by following these steps:</b>
- Download the necessary files with `git clone https://github.com/astra1dev/AUnlocker`
- Run the command `dotnet build` from the AUnlocker folder (where `AUnlocker.sln` is located)
- The compiled mod dll will be located here: `AUnlocker/src/bin/Debug/net6.0/AUnlocker.dll`

<hr>

# 👨‍💻 Contributing

General contribution:
- For bugs and feature suggestions, feel free to open a new issue [here](https://github.com/astra1dev/AUnlocker/issues/new)!
- Feel free to fork this repo and create a pull request ([how?](https://docs.github.com/en/get-started/exploring-projects-on-github/contributing-to-a-project))

Getting started modding Among Us:
- Learn C# and Unity, as well as [BepInEx](https://builds.bepinex.dev/projects/bepinex_be) and Harmony
- Take a look at the [docs](https://docs.reactor.gg) for [Reactor](https://github.com/NuclearPowered/Reactor), a modding API for Among Us. \
  Join their [discord](https://reactor.gg/discord) for the latest `Assembly-CSharp.dll` files that contain the Among Us client source code.
- Take a look at [sus.wiki](https://github.com/roobscoob/among-us-protocol) to learn more about the Among Us network protocol

<hr>

# ⚠️ Disclaimer
This mod is not affiliated with Among Us or Innersloth LLC, and the content contained therein is not endorsed or otherwise sponsored by Innersloth LLC. Portions of the materials contained herein are property of Innersloth LLC. © Innersloth LLC.

This mod does not unlock all cosmetics **permanently**, so it does **not** add them to your account. This is because your progress is stored on the Innersloth servers and we cannot just modify it. However, this mod uses BepInEx and the Reactor Framework to Postfix Among Us game methods and unlock all cosmetics **temporarily**. If you uninstall this mod, the cosmetics will be locked again. The cosmetics you have already unlocked legitimately, e.g. through buying a cosmicube, are **untouched** by this mod.

<hr>

GitHub Stats:
<a href="../../releases/latest">
    <img src="https://img.shields.io/github/release/astra1dev/AUnlocker.svg?label=version&style=plastic">
</a>
<a href="../../releases">
    <img src="https://img.shields.io/github/downloads/astra1dev/AUnlocker/total.svg?style=plastic&color=red">
</a>
