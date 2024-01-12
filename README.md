<p align="center">
  <img src="./icon.png">
</p>

<p align="center">
  <a href="https://learn.microsoft.com/en-us/dotnet/csharp/">
    <img src="https://img.shields.io/badge/Made%20with-C%23-1f425f.svg?style=plastic&logo=csharp&color=000000&labelColor=A200FF">
  </a>
  <a href="https://www.gnu.org/licenses/gpl-3.0.html">
    <img src="https://img.shields.io/badge/license-GPL-brightgreen.svg?style=plastic&logo=GNU&label=License">
  </a>
  <a href="https://github.com/astra1dev/AUnlocker/releases/latest">
    <img src="https://img.shields.io/badge/version-1.1.2-blue.svg?style=plastic&logo=GitHub&color=ff5500&label=Version">
  </a>
</p>

<p align="center">
<b>🔓Unlock Among Us: cosmetics, account, chat and more!🎉</b>


# 🎉 Features

| Unlock Cosmetics | Account Patches        | Chat Patches                 | 
|------------------|------------------------|------------------------------|
| Hats             | Unlock free chat       | No cooldown between messages |
| Visors           | Unlock friend list     | No character limit           |
| Skins            | Unlock custom name     | Copy-pasting allowed         |
| Pets             | Unlock online gameplay | URLs and Emails allowed      |
| Nameplates       | Remove "minor" status  | Special characters allowed   |
| Cosmicubes       |                        |
| Bundles          |                        |

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
| `2023.11.28`       | v1.1.2 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.2) |
| `2023.11.28`       | v1.1.1 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.1) |
| `2023.10.24`       | v1.1.0 [(Download)](https://github.com/astra1dev/AUnlocker/releases/tag/v1.1.0) |


Older versions of Among Us may or may not work. If you want to use AUnlocker with an older version, follow the steps [here](https://github.com/astra1dev/AUnlocker#%EF%B8%8F-build).

<hr>

# 💾 Installation
<details>
  <summary><h3>👶 This is my first time installing an Among Us mod</h3></summary>

## Windows
- Download `AUnlocker_v*.zip` from the latest release found [here](https://github.com/astra1dev/AUnlocker/releases).

- Extract the contents of the zip into your Among Us folder.

    If you have AU on Steam, you can find the folder like this: Right-click AU in your library, click on `manage` and then on `browse local files`
   
- Launch Among Us. The first launch will take **MUCH** longer, so don't worry if you have to wait a few minutes.

## Linux
Check out [this guide](https://docs.bepinex.dev/articles/advanced/proton_wine.html) and [this guide](https://docs.bepinex.dev/master/articles/advanced/steam_interop.html) to get BepInEx (framework AUnlocker is build upon) working. Then follow the steps for Windows.
  
</details>

<details>
  <summary><h3>👴 I installed one or more Among Us mods in the past</h3></summary>

- You should see a folder called `BepInEx` inside your Among Us folder. 
- Download `AUnlocker_v*.dll` from the [latest release](https://github.com/astra1dev/AUnlocker/releases), place it into `BepInEx/plugins` and launch Among Us.
</details>

<hr>

# 👷‍♂️ Build
You can build AUnlocker yourself by following these steps:
- Download the necessary files with `git clone https://github.com/astra1dev/AUnlocker` or with the repo zip file.
- Run the command `dotnet build` from the folder `AUnlocker/source` where the `csproj.` and the `AUnlockerPlugin.cs` files are located
- If you get any errors when building, feel free to open a new issue
- The compiled mod dll will be located here: `AUnlocker/source/bin/Debug/net6.0/AUnlocker.dll`

If you want to use AUnlocker with an older Among Us version, make sure to change this: 
```
<PackageReference Include="AmongUs.GameLibs.Steam" Version="2023.10.24" PrivateAssets="all" />
``` 
in `AUnlocker.csproj` (before building) to this: 
```
<PackageReference Include="AmongUs.GameLibs.Steam" Version="YOUR_DESIRED_VERSION_HERE" PrivateAssets="all" />
```

<hr>

# 🎓 Contributing
- Found a bug? Want to suggest a new feature? Feel free to open a new issue [here](https://github.com/astra1dev/AUnlocker/issues/new)!
- [Here](https://docs.github.com/en/get-started/quickstart/contributing-to-projects) is a great guide if you want to contribute to this project. 
- Here are some resources for making your own Among Us Mod:
  - [Docs](https://docs.reactor.gg) for [Reactor](https://github.com/NuclearPowered/Reactor), a modding API for Among Us. There you will find information on how to patch Among Us methods. \
  If you need help, join their [discord](https://reactor.gg/discord), there you'll also find the latest `Assembly-CSharp.dll` files that contain all Among Us methods.
  - [AUWiki](https://auwiki.duikbo.at/) by Duikboot, documenting the internal workings of Among Us
  - [BepInEx](https://builds.bepinex.dev/projects/bepinex_be), a modding framework for Unity games that is required for Among Us Mods.

<hr>

# ⚠️ Disclaimer
This mod is not affiliated with Among Us or Innersloth LLC, and the content contained therein is not endorsed or otherwise sponsored by Innersloth LLC. Portions of the materials contained herein are property of Innersloth LLC. © Innersloth LLC.

This mod does not unlock all cosmetics **permanently**, so it does **not** add them to your account. This is because your progress is stored on the Innersloth servers and we cannot just modify it. However, this mod uses BepInEx and the Reactor Framework to Postfix Among Us game methods and unlock all cosmetics **temporarily**. If you uninstall this mod, the cosmetics will be locked again. The cosmetics you have already unlocked legitimately, e.g. through buying a cosmicube, are **untouched** by this mod.
