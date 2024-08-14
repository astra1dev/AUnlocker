using HarmonyLib;

namespace AUnlocker;


// Some of the below patches are from https://github.com/scp222thj/MalumMenu/

// UnlockGuest Patches
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFreechatAllowed))]
public static class UnlockFreechat_EOSManager_IsFreechatAllowed_Postfix
{
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockGuest.Value) 
        {
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFriendsListAllowed))]
public static class UnlockFriendlist_EOSManager_IsFriendsListAllowed_Postfix
{
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockGuest.Value)
        {
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(FullAccount), nameof(FullAccount.CanSetCustomName))]
public static class CustomNameEnabled_FullAccount_CanSetCustomName_Prefix
{
    public static void Prefix(ref bool canSetName)
    {
        if (AUnlocker.UnlockGuest.Value)
        {
            canSetName = true;
        }
    }
}

// UnlockMinor Patches
[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsMinorOrWaiting))]
public static class RemoveMinorStatus_EOSManager_IsMinorOrWaiting_Postfix
{
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            __result = false;
        }
    }
}

[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsAllowedOnline))]
public static class OnlineGameplay_EOSManager_IsAllowedOnline_Prefix
{
    public static void Prefix(ref bool canOnline)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            canOnline = true;
        }
    }
}

[HarmonyPatch(typeof(AccountManager), nameof(AccountManager.CanPlayOnline))]
public static class OnlineGameplay_AccountManager_CanPlayOnline_Postfix
{
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            __result = true;
        } 
    }
}

// Change login status to "logged in" to allow joining public games
[HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.JoinGame))]
public static class SetLoggedIn_InnerNetClient_JoinGame_Prefix
{
    public static void Prefix()
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            AmongUs.Data.DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
        }
    }
}

// RemovePenalty Patch
[HarmonyPatch(typeof(StatsManager), nameof(StatsManager.BanMinutesLeft), MethodType.Getter)]
public static class RemoveDisconnectPenalty_StatsManager_BanMinutesLeft_Postfix
{
    public static void Postfix(StatsManager __instance, ref int __result)
    {
        if (AUnlocker.RemovePenalty.Value){
            __instance.BanPoints = 0f; // Remove all BanPoints
            __result = 0;              // Remove all BanMinutes
        }
    }
}