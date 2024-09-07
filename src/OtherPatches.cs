using UnityEngine;
using HarmonyLib;

namespace AUnlocker;

// Set FPS to 165 instead of vanilla 60
[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public static class UnlockFPS_MainMenuManager_Start_Postfix
{
    public static void Postfix(MainMenuManager __instance)
    {
        Application.targetFrameRate = AUnlocker.UnlockFPS.Value;
    }
}


// Show "April Fools' Mode (Limited Cosmetics)" banner (on / off)
// switching it on enables long boi (only client-side)
[HarmonyPatch(typeof(AprilFoolsMode), nameof(AprilFoolsMode.ShouldShowAprilFoolsToggle))]
public static class UnlockAprilFoolsMode_AprilFoolsMode_ShouldShowAprilFoolsToggle_Postfix
{
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
// might be confusing

// Enable horse mode (only client-side)
[HarmonyPatch(typeof(NormalGameManager), nameof(NormalGameManager.GetBodyType))]
public static class EnableHorseMode_NormalGameManager_GetBodyType_Postfix
{
    public static void Postfix(ref PlayerBodyTypes __result)
    {
        if (AUnlocker.EnableHorseMode.Value)
        {
            __result = PlayerBodyTypes.Horse;
            return;
        }        
    }
}
