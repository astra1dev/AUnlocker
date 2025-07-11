using HarmonyLib;

namespace AUnlocker;

[HarmonyPatch(typeof(HatManager), nameof(HatManager.Initialize))]
public static class UnlockCosmetics_HatManager_Initialize_Postfix
{
    // https://github.com/scp222thj/MalumMenu/blob/main/src/Cheats/CosmeticsUnlocker.cs

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
    /// <summary>
    /// Don't show any cosmetics in-game (only client-side).
    /// </summary>
    /// <param name="__instance">The <c>PlayerControl</c> instance.</param>
    public static void Postfix(PlayerControl __instance)
    {
        if (!AUnlocker.DontShowCosmeticsInGame.Value) return;
        __instance.SetHat("", 0);
        __instance.SetSkin("", 0);
        __instance.SetVisor("", 0);
        __instance.SetNamePlate("");
        __instance.SetPet("", 0);
    }
}
