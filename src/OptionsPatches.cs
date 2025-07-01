using HarmonyLib;

namespace AUnlocker;

[HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Increase))]
public static class IncreaseWithoutLimits_NumberOption_Increase_Prefix
{
    /// <summary>
    /// Increase the value of a NumberOption without limits.
    /// </summary>
    /// <param name="__instance">The <c>NumberOption</c> instance.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(NumberOption __instance)
    {
        if (!AUnlocker.NoOptionsLimits.Value) return true;
        __instance.Value +=  __instance.Increment;
        __instance.UpdateValue();
        __instance.OnValueChanged.Invoke(__instance);
        __instance.AdjustButtonsActiveState();
        return false;
    }
}

[HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Decrease))]
public static class DecreaseWithoutLimits_NumberOption_Decrease_Prefix
{
    /// <summary>
    /// Decrease the value of a NumberOption without limits.
    /// </summary>
    /// <param name="__instance">The <c>NumberOption</c> instance.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(NumberOption __instance)
    {
        if (!AUnlocker.NoOptionsLimits.Value) return true;
        __instance.Value -=  __instance.Increment;
        __instance.UpdateValue();
        __instance.OnValueChanged.Invoke(__instance);
        __instance.AdjustButtonsActiveState();
        return false;
    }
}

[HarmonyPatch(typeof(NumberOption), nameof(NumberOption.Initialize))]
public static class UnlimitedRange_NumberOption_Initialize_Postfix
{
    /// <summary>
    /// Set the valid range of a NumberOption to be unlimited.
    /// </summary>
    /// <param name="__instance">The <c>NumberOption</c> instance.</param>
    public static void Postfix(NumberOption __instance)
    {
        if (!AUnlocker.NoOptionsLimits.Value) return;
        __instance.ValidRange = new FloatRange(-999f, 999f);
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
public static class NoAntiCheat_PlayerControl_RpcSyncSettings_Prefix
{
    /// <summary>
    /// Prevent the anti-cheat from kicking you for some settings that are out of the "original" valid range.
    /// </summary>
    /// <param name="__instance">The <c>PlayerControl</c> instance.</param>
    /// <param name="optionsByteArray">The byte array containing the options to sync.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(PlayerControl __instance, byte[] optionsByteArray)
    {
        return !AUnlocker.NoOptionsLimits.Value;
    }
}
