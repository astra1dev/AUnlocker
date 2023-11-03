using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace AUnlocker;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class AUnlockerPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    { 
        Harmony.PatchAll();
    }

    // Cosmetics patch
    [HarmonyPatch(typeof(HatManager), nameof(HatManager.Initialize))]
    public static class UnlockCosmetics
    {
        // Patch HatManager.Initialize to loop through all cosmetics and unlock them
        public static void Postfix(HatManager __instance)
        {
            foreach (var bundle in __instance.allBundles)
            { bundle.Free = true; }

            foreach (var featuredBundle in __instance.allFeaturedBundles)
            { featuredBundle.Free = true; }

            foreach (var featuredCube in __instance.allFeaturedCubes)
            { featuredCube.Free = true; }

            foreach (var featuredItem in __instance.allFeaturedItems)
            { featuredItem.Free = true; }

            foreach (var hat in __instance.allHats)
            { hat.Free = true; }

            foreach (var nameplate in __instance.allNamePlates)
            { nameplate.Free = true; }

            foreach (var pet in __instance.allPets)
            { pet.Free = true; }

            foreach (var skin in __instance.allSkins)
            { skin.Free = true; }

            foreach (var starBundle in __instance.allStarBundles)
            { starBundle.price = 0; }

            foreach (var visor in __instance.allVisors)
            { visor.Free = true; }
        }
    }

    // In the following prefix patches, ref bool __result stores the return value of the original AU method (e.g. IsFreechatAllowed)
    // By setting __result to true, the original method returns true (-> e.g. enables freechat) without executing its usual logic.
    // In the harmony framework, returning false from a prefix method indicates that the original method should not be executed after the prefix.

    // Freechat patch
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFreechatAllowed))]
    public static class UnlockFreechat
    {
        public static bool Prefix(EOSManager __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    // Friends list patch
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFriendsListAllowed))]
    public static class UnlockFriendlist
    {
        public static bool Prefix(EOSManager __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    // Minor status patch
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsMinorOrWaiting))]
    public static class RemoveMinorStatus
    {
        public static bool Prefix(EOSManager __instance, ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    // Online patch
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsAllowedOnline))]
    public static class UnlockOnlineGameplay
    {
        public static bool Prefix(bool canOnline, EOSManager __instance)
        {
            // if online gameplay is disabled...
            if (!canOnline)
            {
                __instance.IsAllowedOnline(true);
                return false;
            }

            // otherwise if online gameplay is already enabled, do nothing
            return true;
        }
    }

    // Custom name patch
    [HarmonyPatch(typeof(FullAccount), nameof(FullAccount.CanSetCustomName))]
    public static class CustomNameEnabled
    {
        public static bool Prefix(bool canSetName, FullAccount __instance)
        {
            // if custom name is disabled...
            if (!canSetName)
            {
                __instance.CanSetCustomName(true);
                return false;
            }

            // otherwise if custom name is already enabled, do nothing
            return true;
        }
    }
}
