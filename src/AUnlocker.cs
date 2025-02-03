using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine.Analytics;

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

    // Other
    public static ConfigEntry<int> UnlockFPS;
    public static ConfigEntry<bool> NoTelemetry;
    public static ConfigEntry<bool> UnlockAprilFoolsMode;
    public static ConfigEntry<bool> EnableHorseMode;

    // Unsafe
    public static ConfigEntry<bool> NoCharacterLimit;
    public static ConfigEntry<bool> NoChatCooldown;

    public override void Load()
    { 
        // Account
        UnlockGuest = Config.Bind("Account", "RemoveGuestStatus", false, "Remove guest restrictions (no custom name, no freechat, no friendlist)");
        UnlockMinor = Config.Bind("Account", "RemoveMinorStatus", false, "Remove minor status and restrictions (no online play)");
        RemovePenalty = Config.Bind("Account", "NoDisconnectPenalty", true, "Remove the penalty after disconnecting from too many lobbies");
        // Chat
        PatchChat = Config.Bind("Chat", "Enabled", true, "Allow all characters\nAllow copy-pasting\nSet character limit to 120");
        // Cosmetics
        UnlockCosmetics = Config.Bind("Cosmetics", "UnlockAll", true, "Unlocks all cosmetics");
        // Other
        UnlockFPS = Config.Bind("Other", "FPS", 60, "Enter how many FPS you want");
        NoTelemetry = Config.Bind("Other", "NoTelemetry", true, "Stop the game from collecting analytics and sending them to Innersloth");
        UnlockAprilFoolsMode = Config.Bind("Other", "UnlockAprilFoolsMode", false, "Add the ability to enable Long Boi Mode (only client-side)");
        EnableHorseMode = Config.Bind("Other", "EnableHorseMode", false, "Enable Horse Mode (only client-side)");
        // Unsafe
        NoCharacterLimit = Config.Bind("Unsafe", "NoCharacterLimit", false, "THESE ARE UNSAFE AND CAN GET YOU KICKED BY ANTICHEAT, USE WITH CAUTION\n\nRemove the character limit in chat");
        NoChatCooldown = Config.Bind("Unsafe", "NoChatCooldown", false, "Remove the 3s chat cooldown");

        Harmony.PatchAll();

        if (NoTelemetry.Value) {
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
}
