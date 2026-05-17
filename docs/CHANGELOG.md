# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- New CHANGELOG.md file
- Recognize Starlight platform when finding a game.
  Previously, if MoreLobbyInfo was enabled, lobbies where the host is playing on Starlight would show "Unknown" as platform

### Changed

- Bump AmongUs.GameLibs.Steam from 2025.11.18 to 2026.3.31 by [@dependabot](https://github.com/dependabot) (#118)
- Bump actions/upload-artifact from 6 to 7 by [@dependabot](https://github.com/dependabot) (#114)

## [1.3.0] - 2026-01-30

This release is compatible with Among Us version `17.1.0` (`2025.11.18`).

### Added

- Show the exact number of lobbies online
- Reduce the chat cooldown from 3s to 2.1s by [@ApeMV](https://github.com/ApeMV) (#87)
- Always display the timer in the bottom left corner to indicate when the server will close the lobby (Works only as Host)
- Starlight support by [@XtraCube](https://github.com/XtraCube) (#105)

### Changed

- Bump Harmonize from 1.0.5 to 1.0.6 by [@dependabot](https://github.com/dependabot) (#106)
- Bump actions/upload-artifact from 4 to 6 by [@dependabot](https://github.com/dependabot) (#96)
- Bump actions/checkout from 3 to 6 by [@dependabot](https://github.com/dependabot) (#98)
- Bump cake-build/cake-action from 1 to 3 by [@dependabot](https://github.com/dependabot) (#97)
- Bump actions/setup-dotnet from 3 to 5 by [@dependabot](https://github.com/dependabot) (#99)

### Fixed

- Avoid changing game options that may kick the user by [@ApeMV](https://github.com/ApeMV) (#90)

### Removed

- Remove `IsCharAllowed` patch as it's causing issues with chinese characters by [@astra1dev](https://github.com/astra1dev) (#102)

## [1.2.2] - 2025-09-07

This release is compatible with Among Us version `16.1.0` (`2025.6.10`), `17.0.0` (`2025.9.9`), `17.0.1` (`2025.10.14`) and `17.1.0` (`2025.11.18`).

### Added

- Resize the in-game buttons (like Use, Kill, Report, etc.)
- Modify the maximum amount of messages to keep in the chat history
- Scroll through previous chat messages using up and down arrow keys (#69)

### Changed

- Unlock Cosmetics now shows all bundles as owned and cosmicubes as 100% completed (before, cosmicubes would show as 0% completion even tho the cosmetics from it are unlocked and usable)
- Unlock April Fools Mode and Enable Horse Mode are now combined together with Seeker and LongHorse into one April Fools Mode option (#63)

### Fixed

- Fix Don't show cosmetics in-game freezing the game (#62)

## [1.2.1] - 2025-07-03

This release is compatible with Among Us version `16.1.0` (`2025.6.10`).

### Added

- Don't show cosmetics in-game (#60)
- No Options Limits (#54, #56)
- Keybind to reload the config file while in-game

### Fixed

- Fix pasting unicode characters into chat (#38)

## [1.2.0] - 2025-05-20

This release is compatible with Among Us version `16.0.5` (`2025.5.20`).

### Added

- More Lobby Info
- Show Task Panel in Meetings

### Changed

- Improve No disconnect penalty (#45)

## [1.1.8] - 2025-03-25

This release is compatible with Among Us version `16.0.0` (`2025.3.25`).

### Added

- `Unsafe` category:
  - Allow All Characters
  - No Character Limit
  - No Chat Cooldown

### Changed

- Better configuration options
- Improve No Telemetry
- Rework character limit patch

### Fixed

- Fix No disconnect penalty causing plugin to fail loading after game update (#41)

## [1.1.7] - 2024-09-07

This release is compatible with Among Us version `2024.9.4`, `2024.10.29` and `2024.11.26`.

### Added

- `steam_appid.txt` to zip
- Support for Microsoft Store

### Changed

- FPS can now be set to an int value (e.g. 165)

### Fixed

- Fix chat messages auto-sending (#25)

## [1.1.6] - 2024-08-14

This release is compatible with Among Us version `2024.8.13`.

### Added

- More configuration options
- Remove disconnect penalty
- Unlock FPS
- No Telemetry
- Unlock April Fools Mode

## [1.1.5] - 2024-07-22

This release is compatible with Among Us version `2024.6.18`.

### Added

- Config system so users can set which patches they want enabled / disabled.

## [1.1.4] - 2024-03-06

This release is compatible with Among Us version `2024.3.05`.

### Changed

- Update AUnlocker to work with the newest Among Us version.

## [1.1.3] - 2024-01-24

This release is compatible with Among Us version `2023.11.28`.

### Removed

Removed because Innersloth upgraded their anti-cheat:

- No cooldown between messages
- No character limit

## [1.1.2] - 2023-12-31

This release is compatible with Among Us version `2023.11.28`.

### Added

- Add chat-related patches:
  - No cooldown between messages
  - No character limit
  - Copy-pasting allowed
  - URLs and Emails allowed
  - Special characters allowed

## [1.1.1] - 2023-11-29

This release is compatible with Among Us version `2023.11.28`.

### Changed

- Update AUnlocker to work with the newest Among Us version.

## [1.1.0] - 2023-11-02

This release is compatible with Among Us version `2023.10.24`.

### Added

- Unlockers for: Free chat, friends list, online gameplay and custom name
- "Minor" status remover

### Changed

### Fixed

- Fix AUnlocker only working with an older Among Us version
- Fix annoying popup every time when playing public lobbies

### Removed

- Unused Reactor dependency

[unreleased]: https://github.com/astra1dev/AUnlocker/compare/v1.3.0...HEAD
[1.3.0]: https://github.com/astra1dev/AUnlocker/compare/v1.2.2...v1.3.0
[1.2.2]: https://github.com/astra1dev/AUnlocker/compare/v1.2.1...v1.2.2
[1.2.1]: https://github.com/astra1dev/AUnlocker/compare/v1.2.0...v1.2.1
[1.2.0]: https://github.com/astra1dev/AUnlocker/compare/v1.1.8...v1.2.0
[1.1.8]: https://github.com/astra1dev/AUnlocker/compare/v1.1.7...v1.1.8
[1.1.7]: https://github.com/astra1dev/AUnlocker/compare/v1.1.6...v1.1.7
[1.1.6]: https://github.com/astra1dev/AUnlocker/compare/v1.1.5...v1.1.6
[1.1.5]: https://github.com/astra1dev/AUnlocker/compare/v1.1.4...v1.1.5
[1.1.4]: https://github.com/astra1dev/AUnlocker/compare/v1.1.3...v1.1.4
[1.1.3]: https://github.com/astra1dev/AUnlocker/compare/v1.1.2...v1.1.3
[1.1.2]: https://github.com/astra1dev/AUnlocker/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/astra1dev/AUnlocker/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/astra1dev/AUnlocker/compare/v1.1...v1.1.0
