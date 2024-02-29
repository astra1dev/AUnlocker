using HarmonyLib;

namespace AUnlocker;


// Some of the below patches are from https://github.com/scp222thj/MalumMenu/blob/main/src/Passive/UnlockFeaturesPatch.cs
// Unlock freechat
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFreechatAllowed))]
public static class UnlockFreechat
{
    public static bool Prefix(EOSManager __instance, ref bool __result)
    {
        __result = true;
        return false;
    }
}


// Unlock friend list
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFriendsListAllowed))]
public static class UnlockFriendlist
{
    public static bool Prefix(EOSManager __instance, ref bool __result)
    {
        __result = true;
        return false;
    }
}


// Remove minor status
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsMinorOrWaiting))]
public static class RemoveMinorStatus
{
    public static bool Prefix(EOSManager __instance, ref bool __result)
    {
        __result = false;
        return false;
    }
}


// Unlock online gameplay
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsAllowedOnline))]
public static class OnlineGameplay
{
    public static void Prefix(ref bool canOnline)
    {
        canOnline = true;
    }
}


[HarmonyPatch(typeof(AccountManager), nameof(AccountManager.CanPlayOnline))]
public static class OnlineGameplay2
{
    public static void Postfix(ref bool __result)
    {
            __result = true;     
    }
}


// Change login status to "logged in" to allow joining public games
[HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.JoinGame))]
public static class CanJoinGame
{
    public static void Prefix()
    {
        AmongUs.Data.DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
    }
}


// Unlock custom name
[HarmonyPatch(typeof(FullAccount), nameof(FullAccount.CanSetCustomName))]
public static class CustomNameEnabled
{
    public static void Prefix(ref bool canSetName)
    {
        canSetName = true;
    }
}
