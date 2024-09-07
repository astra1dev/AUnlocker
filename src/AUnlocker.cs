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

    // AccountPatches
    public static ConfigEntry<bool> UnlockGuest;
    public static ConfigEntry<bool> UnlockMinor;
    public static ConfigEntry<bool> RemovePenalty;

    // ChatPatches
    public static ConfigEntry<bool> PatchChat;

    // CosmeticPatches
    public static ConfigEntry<bool> UnlockCosmetics;

    // OtherPatches
    public static ConfigEntry<int> UnlockFPS;
    public static ConfigEntry<bool> NoTelemetry;
    public static ConfigEntry<bool> UnlockAprilFoolsMode;
    public static ConfigEntry<bool> EnableHorseMode;

    public override void Load()
    { 
        // AccountPatches
        UnlockGuest = Config.Bind("AccountPatches", "UnlockGuest", false, "Remove guest restrictions (no custom name, no freechat, no friendlist)");
        UnlockMinor = Config.Bind("AccountPatches", "UnlockMinor", false, "Remove minor status and restrictions (no online play)");
        RemovePenalty = Config.Bind("AccountPatches", "RemovePenalty", true, "Remove the penalty after disconnecting from too many lobbies");
        // ChatPatches
        PatchChat = Config.Bind("ChatPatches", "ChatPatches", true, "Enable chat-related patches");
        // CosmeticPatches
        UnlockCosmetics = Config.Bind("CosmeticPatches", "CosmeticPatches", true, "Unlocks all cosmetics");
        // OtherPatches
        UnlockFPS = Config.Bind("OtherPatches", "UnlockFPS", 60, "Enter how many FPS you want");
        NoTelemetry = Config.Bind("OtherPatches", "NoTelemetry", true, "Stop the game from collecting analytics and sending them to Innersloth");
        UnlockAprilFoolsMode = Config.Bind("OtherPatches", "UnlockAprilFoolsMode", false, "Add the ability to enable Long Boi Mode (only client-side)");
        EnableHorseMode = Config.Bind("OtherPatches", "EnableHorseMode", false, "Enable Horse Mode (only client-side)");

        Harmony.PatchAll();

        if (NoTelemetry.Value) {
            Analytics.enabled = false;
            Analytics.deviceStatsEnabled = false;
            PerformanceReporting.enabled = false;
        }
    }
}
