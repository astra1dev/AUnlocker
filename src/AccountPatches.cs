using AmongUs.Data.Player;
using HarmonyLib;

namespace AUnlocker;

// Some of the below patches are from https://github.com/scp222thj/MalumMenu/

[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsFreechatAllowed))]
public static class UnlockFreechat_EOSManager_IsFreechatAllowed_Postfix
{
    /// <summary>
    /// Allow creating and joining free chat lobbies instead of quick chat only.
    /// </summary>
    /// <param name="__result">Original return value of <c>IsFreechatAllowed</c>.</param>
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
    /// <summary>
    /// Allow using the friends list.
    /// </summary>
    /// <param name="__result">Original return value of <c>IsFriendsListAllowed</c>.</param>
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
    /// <summary>
    /// Allow setting a custom name.
    /// </summary>
    /// <param name="canSetName">Original return value of <c>CanSetCustomName</c>.</param>
    public static void Prefix(ref bool canSetName)
    {
        if (AUnlocker.UnlockGuest.Value)
        {
            canSetName = true;
        }
    }
}

[HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsMinorOrWaiting))]
public static class RemoveMinorStatus_EOSManager_IsMinorOrWaiting_Postfix
{
    /// <summary>
    /// Remove "minor" status.
    /// </summary>
    /// <param name="__result">Original return value of <c>IsMinorOrWaiting</c>.</param>
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
    /// <summary>
    /// Allow creating and joining online lobbies.
    /// </summary>
    /// <param name="canOnline">Original return value of <c>IsAllowedOnline</c>.</param>
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
    /// <summary>
    /// Allow creating and joining online lobbies.
    /// </summary>
    /// <param name="__result">Original return value of <c>CanPlayOnline</c>.</param>
    public static void Postfix(ref bool __result)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(InnerNet.InnerNetClient), nameof(InnerNet.InnerNetClient.JoinGame))]
public static class SetLoggedIn_InnerNetClient_JoinGame_Prefix
{
    /// <summary>
    /// Set the login status to "logged in" to allow joining public games.
    /// </summary>
    public static void Prefix()
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            AmongUs.Data.DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
        }
    }
}

[HarmonyPatch(typeof(PlayerBanData), nameof(PlayerBanData.BanMinutesLeft), MethodType.Getter)]
public static class RemoveDisconnectPenalty_PlayerBanData_BanMinutesLeft_Postfix
{
    /// <summary>
    /// Remove the time penalty after disconnecting from too many lobbies.
    /// </summary>
    /// <param name="__instance">The <c>PlayerBanData</c> instance.</param>
    /// <param name="__result">Original return value of <c>BanMinutesLeft</c>.</param>
    public static void Postfix(PlayerBanData __instance, ref int __result)
    {
        if (!AUnlocker.RemovePenalty.Value) return;

        __instance.BanPoints = 0f; // Remove all BanPoints
        __result = 0;              // Remove all BanMinutes
    }
}
