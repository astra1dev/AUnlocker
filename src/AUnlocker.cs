using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine.Analytics;
using UnityEngine.CrashReportHandler;

namespace AUnlocker;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class AUnlocker : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    // Account
    public static ConfigEntry<bool> UnlockGuest;
    public static ConfigEntry<bool> UnlockMinor;
    public static ConfigEntry<bool> RemovePenalty;

    // Chat
    public static ConfigEntry<bool> PatchChat;

    // Cosmetics
    public static ConfigEntry<bool> UnlockCosmetics;
    public static ConfigEntry<bool> DontShowCosmeticsInGame;

    // Other
    public static ConfigEntry<int> UnlockFPS;
    public static ConfigEntry<bool> DisableTelemetry;
    public static ConfigEntry<bool> UnlockAprilFoolsMode;
    public static ConfigEntry<bool> EnableHorseMode;
    public static ConfigEntry<bool> MoreLobbyInfo;
    public static ConfigEntry<bool> ShowTaskPanelInMeetings;

    // Unsafe
    public static ConfigEntry<bool> AllowAllCharacters;
    public static ConfigEntry<bool> NoCharacterLimit;
    public static ConfigEntry<bool> NoChatCooldown;
    public static ConfigEntry<bool> NoOptionsLimits;

    /// <summary>
    /// Initialize the plugin, set up configuration entries and apply patches.
    /// </summary>
    public override void Load()
    {
        // Account
        UnlockGuest = Config.Bind("Account", "RemoveGuestStatus", false, "Remove guest restrictions (no custom name, no free chat, no friend list)");
        UnlockMinor = Config.Bind("Account", "RemoveMinorStatus", false, "Remove minor status and restrictions (no online play)");
        RemovePenalty = Config.Bind("Account", "NoDisconnectPenalty", true, "Remove the penalty after disconnecting from too many lobbies");
        // Chat
        PatchChat = Config.Bind("Chat", "Enabled", true, "Allow Ctrl+C and Ctrl+V (copy-pasting)\nBe able to send URLs and Email addresses\nIncrease the character limit from 100 to 120");
        // Cosmetics
        UnlockCosmetics = Config.Bind("Cosmetics", "UnlockAll", true, "Unlock all cosmetics");
        DontShowCosmeticsInGame = Config.Bind("Cosmetics", "DontShowCosmeticsInGame", false, "Don't show any cosmetics in-game (only client-side)");
        // Other
        UnlockFPS = Config.Bind("Other", "FPS", 60, "Set the game's FPS cap to this value");
        DisableTelemetry = Config.Bind("Other", "DisableTelemetry", true, "Prevent the game from collecting analytics and sending them to Innersloth");
        UnlockAprilFoolsMode = Config.Bind("Other", "UnlockAprilFoolsMode", false, "Add the ability to enable Long Boi Mode (only client-side)");
        EnableHorseMode = Config.Bind("Other", "EnableHorseMode", false, "Enable Horse Mode (only client-side)");
        MoreLobbyInfo = Config.Bind("Other", "MoreLobbyInfo", false, "Show more information when finding a game: host name (e.g. Astral), lobby code (e.g. KLHCEG), host platform (e.g. Epic), and lobby age in minutes (e.g. 4:20)");
        ShowTaskPanelInMeetings = Config.Bind("Other", "ShowTaskPanelInMeetings", false, "Show the task panel (contains a list of your tasks) during meetings");
        // Unsafe
        AllowAllCharacters = Config.Bind("Unsafe", "AllowAllCharacters", false, "THESE ARE UNSAFE AND CAN GET YOU KICKED BY ANTI-CHEAT, USE WITH CAUTION\n\nBe able to send any character in chat");
        NoCharacterLimit = Config.Bind("Unsafe", "NoCharacterLimit", false, "No character limit in chat");
        NoChatCooldown = Config.Bind("Unsafe", "NoChatCooldown", false, "No 3s cooldown between chat messages");
        NoOptionsLimits = Config.Bind("Unsafe", "NoOptionsLimits", false, "No limits on game host options (e.g. amount of impostors)");

        Harmony.PatchAll();

        if (!DisableTelemetry.Value) return;
        Analytics.deviceStatsEnabled = false;
        Analytics.enabled = false;
        Analytics.initializeOnStartup = false;
        Analytics.limitUserTracking = true;
        CrashReportHandler.enableCaptureExceptions = false;
        PerformanceReporting.enabled = false;

        // If Among Us updates their IAP / Analytics system, we will need to use this:
            // using Unity.Services.Analytics;
            // using Unity.Services.Core;
            // AnalyticsService.Instance.OptOut();
        // More Info: https://discussions.unity.com/t/iap-privacy-issue/881743
    }
}
