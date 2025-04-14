using UnityEngine;
using HarmonyLib;
using InnerNet;

namespace AUnlocker;

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public static class UnlockFPS_MainMenuManager_Start_Postfix
{
    /// <summary>
    /// Set the target frame rate based on the config setting.
    /// </summary>
    public static void Postfix()
    {
        Application.targetFrameRate = AUnlocker.UnlockFPS.Value;
    }
}

[HarmonyPatch(typeof(AprilFoolsMode), nameof(AprilFoolsMode.ShouldShowAprilFoolsToggle))]
public static class UnlockAprilFoolsMode_AprilFoolsMode_ShouldShowAprilFoolsToggle_Postfix
{
    /// <summary>
    /// Force the "April Fools' Mode (Limited Cosmetics)" banner to be visible.
    /// Enabling it activates long boi mode (client-side only).
    /// </summary>
    /// <param name="__result">Original return value of <c>ShouldShowAprilFoolsToggle</c>.</param>
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockAprilFoolsMode.Value)
        {
            // Remove check if the current server time is within the first week of april
            __result = true;
        }
    }
}

// maybe remove the below patch and patch ShouldHorseAround instead
// this will probably allow horse mode to work in HnS properly

// maybe also remove the above patch and patch ShouldLongAround instead
[HarmonyPatch(typeof(NormalGameManager), nameof(NormalGameManager.GetBodyType))]
public static class EnableHorseMode_NormalGameManager_GetBodyType_Postfix
{
    /// <summary>
    /// Set the player's body type to Horse when the horse mode config setting is enabled. (client-side only)
    /// </summary>
    /// <param name="__result">Original return value of <c>GetBodyType</c>.</param>
    public static void Postfix(ref PlayerBodyTypes __result)
    {
        if (!AUnlocker.EnableHorseMode.Value) return;
        __result = PlayerBodyTypes.Horse;
    }
}

// https://github.com/g0aty/SickoMenu/blob/main/hooks/LobbyBehaviour.cpp
[HarmonyPatch(typeof(GameContainer), nameof(GameContainer.SetupGameInfo))]
public static class MoreLobbyInfo_GameContainer_SetupGameInfo_Postfix
{
    /// <summary>
    /// Show more information when finding a game:
    /// host name (e.g. Astral), lobby code (e.g. KLHCEG), host platform (e.g. Epic), and lobby age in minutes (e.g. 4:20)
    /// </summary>
    /// <param name="__instance">The <c>GameContainer</c> instance.</param>
    public static void Postfix(GameContainer __instance)
    {
        if (!AUnlocker.MoreLobbyInfo.Value) return;

        var trueHostName = __instance.gameListing.TrueHostName;

        // The Crewmate icon gets aligned properly with this
        const string separator = "<#0000>000000000000000</color>";

        var age = __instance.gameListing.Age;
        var lobbyTime = $"Age: {age / 60}:{(age % 60 < 10 ? "0" : "")}{age % 60}";

        var platformId = __instance.gameListing.Platform switch
        {
            Platforms.StandaloneEpicPC => "Epic",
            Platforms.StandaloneSteamPC => "Steam",
            Platforms.StandaloneMac => "Mac",
            Platforms.StandaloneWin10 => "Microsoft Store",
            Platforms.StandaloneItch => "Itch.io",
            Platforms.IPhone => "iPhone / iPad",
            Platforms.Android => "Android",
            Platforms.Switch => "Nintendo Switch",
            Platforms.Xbox => "Xbox",
            Platforms.Playstation => "PlayStation",
            _ => "Unknown"
        };
        // Set the text of the capacity field to include the new information
        __instance.capacity.text = $"<size=40%>{separator}\n{trueHostName}\n{__instance.capacity.text}\n" +
                                   $"<#fb0>{GameCode.IntToGameName(__instance.gameListing.GameId)}</color>\n" +
                                   $"<#b0f>{platformId}</color>\n{lobbyTime}\n{separator}</size>";
    }
}
