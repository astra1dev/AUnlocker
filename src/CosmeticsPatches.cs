using System;
using HarmonyLib;
using System.Collections.Generic;

namespace AUnlocker;

[HarmonyPatch(typeof(HatManager), nameof(HatManager.Initialize))]
public static class UnlockCosmetics_HatManager_Initialize_Postfix
{
    // https://github.com/scp222thj/MalumMenu/blob/main/src/Cheats/CosmeticsUnlocker.cs

    /// <summary>
    /// Ensure the patch is not applied on Android.
    /// </summary>
    /// <returns>True if the patch should be applied; otherwise, false.</returns>
    public static bool Prepare()
    {
        return !OperatingSystem.IsAndroid();
    }

    /// <summary>
    /// Unlock all cosmetics by setting their price to 0 and marking them as free.
    /// </summary>
    /// <param name="__instance">The <c>HatManager</c> instance.</param>
    public static void Postfix(HatManager __instance)
    {
        if (!AUnlocker.UnlockCosmetics.Value) return;

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

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
public static class DontShowCosmeticsInGame_PlayerControl_FixedUpdate_Postfix
{    
    private static readonly Dictionary<byte, float> LastCheckTime = new();
    
    /// <summary>
    /// Don't show any cosmetics in-game (only client-side).
    /// </summary>
    /// <param name="__instance">The <c>PlayerControl</c> instance.</param>
    public static void Postfix(PlayerControl __instance)
    {
        if (!AUnlocker.DontShowCosmeticsInGame.Value)
        {
            LastCheckTime.Clear();
            return;
        }

        if (__instance == null) return;
        if (__instance.Data == null) return;

        var outfit = __instance.Data.DefaultOutfit;
        if (outfit == null) return;
        if (outfit.IsIncomplete) return;

        float now = UnityEngine.Time.time;

        if (LastCheckTime.TryGetValue(__instance.PlayerId, out float lastTime))
        {
            if (now - lastTime < 0.25f)
                return;
        }

        LastCheckTime[__instance.PlayerId] = now;

        bool hasHat = HasCosmetic(outfit.HatId);
        bool hasSkin = HasCosmetic(outfit.SkinId);
        bool hasVisor = HasCosmetic(outfit.VisorId);
        bool hasPet = HasCosmetic(outfit.PetId);
        bool hasNamePlate = HasCosmetic(outfit.NamePlateId);

        if (!hasHat && !hasSkin && !hasVisor && !hasPet && !hasNamePlate)
            return;

        try
        {
            if (hasHat && !string.IsNullOrEmpty(HatData.EmptyId))
                __instance.SetHat(HatData.EmptyId, outfit.ColorId);

            if (hasSkin && !string.IsNullOrEmpty(SkinData.EmptyId))
                __instance.SetSkin(SkinData.EmptyId, outfit.ColorId);

            if (hasVisor && !string.IsNullOrEmpty(VisorData.EmptyId))
                __instance.SetVisor(VisorData.EmptyId, outfit.ColorId);

            if (hasNamePlate)
                __instance.SetNamePlate("");

            if (hasPet && !string.IsNullOrEmpty(PetData.EmptyId))
                __instance.SetPet(PetData.EmptyId, outfit.ColorId);
        }
        catch
        {
        }
    }

    private static bool HasCosmetic(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;

        string missingId = NetworkedPlayerInfo.PlayerOutfit.MISSING_COSMETIC_ID;

        if (!string.IsNullOrEmpty(missingId) && id == missingId)
            return false;

        return true;
    }
}
[HarmonyPatch(typeof(PlayerPurchasesData), nameof(PlayerPurchasesData.GetPurchase))]
public static class UnlockCosmetics_PlayerPurchasesData_GetPurchase_Prefix
{
    /// <summary>
    /// Ensure the patch is not applied on Android.
    /// </summary>
    /// <returns>True if the patch should be applied; otherwise, false.</returns>
    public static bool Prepare()
    {
        return !OperatingSystem.IsAndroid();
    }

    public static bool Prefix(PlayerPurchasesData __instance, string itemKey, string bundleKey, ref bool __result)
    {
        if (!AUnlocker.UnlockCosmetics.Value) return true;
        __result = true;
        return false;
    }
}
