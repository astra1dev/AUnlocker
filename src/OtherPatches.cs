using System;
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

/// <summary>
/// Set the player's body type according to the <c>AprilFoolsMode</c> config option (Horse, Long, or LongHorse). (client-side only)
/// https://github.com/AU-Avengers/TOU-Mira/blob/dev/TownOfUs/Patches/AprilFools/AprilFools.cs#L115
/// </summary>
[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.BodyType), MethodType.Getter)]
public static class AprilFoolsMode_PlayerControl_BodyType_Prefix
{
    public static bool Prefix(ref PlayerBodyTypes __result)
    {
        switch (AUnlocker.AprilFoolsMode.Value)
        {
            case "Horse":
                __result = PlayerBodyTypes.Horse;
                return false;
            case "Long":
                __result = PlayerBodyTypes.Long;
                return false;
            case "LongHorse":
                __result = PlayerBodyTypes.LongSeeker;
                return false;
            default:
                return true;
        }
    }
}

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.SetBodyType))]
public static class AprilFoolsMode_PlayerPhysics_SetBodyType_Prefix
{
    public static void Prefix(ref PlayerBodyTypes bodyType)
    {
        bodyType = AUnlocker.AprilFoolsMode.Value switch
        {
            "Horse" => PlayerBodyTypes.Horse,
            "Long" => PlayerBodyTypes.Long,
            "LongHorse" => PlayerBodyTypes.LongSeeker,
            _ => bodyType
        };
    }
}

// Remove checks for ShouldLongAround() in LongBoiPlayerBody.Awake() and .Start()
// Patching ShouldLongAround() directly doesn't work anymore for some reason
[HarmonyPatch(typeof(LongBoiPlayerBody), nameof(LongBoiPlayerBody.Awake))]
public static class AprilFoolsMode_LongBoiPlayerBody_Awake_Prefix
{
    public static bool Prefix(LongBoiPlayerBody __instance)
    {
        __instance.cosmeticLayer.OnSetBodyAsGhost += (Action)__instance.SetPoolableGhost;
        __instance.cosmeticLayer.OnColorChange += (Action<int>)__instance.SetHeightFromColor;
        __instance.cosmeticLayer.OnCosmeticSet += (Action<string, int, CosmeticsLayer.CosmeticKind>)__instance.OnCosmeticSet;
        return false;
    }
}

[HarmonyPatch(typeof(LongBoiPlayerBody), nameof(LongBoiPlayerBody.Start))]
public static class AprilFoolsMode_LongBoiPlayerBody_Start_Prefix
{
    public static bool Prefix(LongBoiPlayerBody __instance)
    {
        __instance.ShouldLongAround = true;
        if (__instance.hideCosmeticsQC) __instance.cosmeticLayer.SetHatVisorVisible(false);
        __instance.SetupNeckGrowth();
        if (__instance.isExiledPlayer)
        {
            var instance = ShipStatus.Instance;
            if (!instance || instance.Type != ShipStatus.MapType.Fungle)
            {
                __instance.cosmeticLayer.AdjustCosmeticRotations(-17.75f);
            }
        }
        if (!__instance.isPoolablePlayer) __instance.cosmeticLayer.ValidateCosmetics();
        return false;
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

[HarmonyPatch(typeof(HudManager))]
[HarmonyPatch("SetHudActive", typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool))]
public static class ShowTaskPanelInMeetings_HudManager_SetHudActive
{
    /// <summary>
    /// Show the task panel (contains a list of your tasks) during meetings.
    /// </summary>
    /// <param name="__instance">The <c>HudManager</c> instance.</param>
    /// <param name="role">The role of the player.</param>
    /// <param name="isActive">Whether to set the Hud to active or inactive.</param>
    public static void Postfix(HudManager __instance, RoleBehaviour role, bool isActive)
    {
        if (!AUnlocker.ShowTaskPanelInMeetings.Value) return;
        if (!MeetingHud.Instance) return;

        // Modify openPosition so the task panel appears on top of the meeting screen
        var openPosition = __instance.TaskPanel.openPosition;
        openPosition.z = -20f;
        __instance.TaskPanel.openPosition = openPosition;

        __instance.TaskPanel.gameObject.SetActive(true);
    }
}
