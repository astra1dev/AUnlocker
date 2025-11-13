using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.CrashReportHandler;

namespace AUnlocker;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class AUnlocker : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    // General
    public static ConfigEntry<KeyCode> ReloadConfigKeybind;
    public static ConfigEntry<float> ButtonSize;

    // Account
    public static ConfigEntry<bool> UnlockGuest;
    public static ConfigEntry<bool> UnlockMinor;
    public static ConfigEntry<bool> RemovePenalty;

    // Chat
    public static ConfigEntry<bool> PatchChat;
    public static ConfigEntry<int> ChatHistoryLimit;

    // Cosmetics
    public static ConfigEntry<bool> UnlockCosmetics;
    public static ConfigEntry<bool> DontShowCosmeticsInGame;

    // Other
    public static ConfigEntry<int> UnlockFPS;
    public static ConfigEntry<bool> DisableTelemetry;
    public static ConfigEntry<string> AprilFoolsMode;
    public static ConfigEntry<bool> MoreLobbyInfo;
    public static ConfigEntry<bool> AlwaysShowLobbyTimer;
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
        // General
        ReloadConfigKeybind = Config.Bind("General", "ReloadConfigKeybind", KeyCode.F6, "The keyboard key used to reload the configuration file");
        ButtonSize = Config.Bind("General", "ButtonSize", 1f, "Resize the in-game buttons (Use, Kill, Report, etc.)\nSet to 1.0 to disable scaling");
        // Account
        UnlockGuest = Config.Bind("Account", "RemoveGuestStatus", false, "Remove guest restrictions (no custom name, no free chat, no friend list)");
        UnlockMinor = Config.Bind("Account", "RemoveMinorStatus", false, "Remove minor status and restrictions (no online play)");
        RemovePenalty = Config.Bind("Account", "NoDisconnectPenalty", true, "Remove the penalty after disconnecting from too many lobbies");
        // Chat
        PatchChat = Config.Bind("Chat", "Enabled", true, "Allow Ctrl+C and Ctrl+V (copy-pasting)\nBe able to send URLs and Email addresses\nIncrease the character limit from 100 to 120");
        ChatHistoryLimit = Config.Bind("Chat", "ChatHistoryLimit", 20, "The maximum amount of chat messages to keep in the chat history");
        // Cosmetics
        UnlockCosmetics = Config.Bind("Cosmetics", "UnlockAll", true, "Unlock all cosmetics");
        DontShowCosmeticsInGame = Config.Bind("Cosmetics", "DontShowCosmeticsInGame", false, "Don't show any cosmetics in-game (only client-side)");
        // Other
        UnlockFPS = Config.Bind("Other", "FPS", 60, "Set the game's FPS cap to this value");
        DisableTelemetry = Config.Bind("Other", "DisableTelemetry", true, "Prevent the game from collecting analytics and sending them to Innersloth");
        AprilFoolsMode = Config.Bind("Other", "AprilFoolsMode", "Disabled", "Enable April Fools Mode (only client-side)\n\nOptions: Disabled, Horse, Seeker, Long, LongHorse");
        MoreLobbyInfo = Config.Bind("Other", "MoreLobbyInfo", false, "Show more information when finding a game: host name (e.g. Astral), lobby code (e.g. KLHCEG), host platform (e.g. Epic), " +
                                                                     "and lobby age in minutes (e.g. 4:20).\nAdditionally, display the exact number of lobbies online instead of an approximation like '500+'");
        AlwaysShowLobbyTimer = Config.Bind("Other", "AlwaysShowLobbyTimer", false, "Always display the timer in the bottom left corner to indicate when the server will close the lobby (Works only as Host)");
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

        AddComponent<KeybindListener>().Plugin = this;
        HudManager_Start_Patch.Plugin = this;
        ChatJailbreak_ChatController_Update_Postfix.Plugin = this;
    }
}

public class KeybindListener : MonoBehaviour
{
    public AUnlocker Plugin { get; internal set; }

    public void Update()
    {
        if (!Input.GetKeyDown(AUnlocker.ReloadConfigKeybind.Value)) return;
        Plugin.Config.Reload();
        Plugin.Log.LogInfo("Configuration reloaded.");
    }
}

// https://github.com/AU-Avengers/TOU-Mira/blob/main/TownOfUs/Patches/HudManagerPatches.cs#L57
public static class Resize
{
    /// <summary>
    /// Resize the in-game buttons (Use, Kill, Report, etc.) based on the given scale factor.
    /// </summary>
    /// <param name="scaleFactor">The scale factor to apply to the buttons.</param>
    public static void ResizeUI(float scaleFactor)
    {
        // Resize the buttons by scaleFactor
        foreach (var button in HudManager.Instance.GetComponentsInChildren<ActionButton>(true))
            button.gameObject.transform.localScale *= scaleFactor;

        // Make sure the buttons have fitting distance between them
        foreach (var arrange in HudManager.Instance.transform.FindChild("Buttons")
                     .GetComponentsInChildren<GridArrange>(true))
            arrange.CellSize *= new Vector2(scaleFactor, scaleFactor);

        // Change DistanceFromEdge for the buttons depending on scaleFactor
        // (closer to the edge for smaller scale factor, further from the edge for larger scale factor)
        foreach (var aspect in HudManager.Instance.transform.FindChild("Buttons")
                     .GetComponentsInChildren<AspectPosition>(true))
        {
            if (aspect.gameObject.transform.parent.name == "TopRight") { continue; }
            if (aspect.gameObject.transform.parent.transform.parent.name == "TopRight") { continue; }
            aspect.gameObject.SetActive(!aspect.isActiveAndEnabled);
            aspect.DistanceFromEdge *= new Vector2(scaleFactor, scaleFactor);
            aspect.gameObject.SetActive(!aspect.isActiveAndEnabled);
        }
    }
}

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
public static class HudManager_Start_Patch
{
    public static AUnlocker Plugin { get; internal set; }
    public static void Postfix()
    {
        Resize.ResizeUI(AUnlocker.ButtonSize.Value);
        Plugin.Log.LogInfo("UI resized to " + AUnlocker.ButtonSize.Value * 100 + "%");
    }
}
