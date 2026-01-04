using AmongUs.Data.Player;
using HarmonyLib;
using Epic.OnlineServices;
using Epic.OnlineServices.KWS;

namespace AUnlocker;

// Some of the below patches are from https://github.com/scp222thj/MalumMenu/

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.QueryAgeGate))]
public static class QueryAgeGate_KWSInterface_QueryAgeGate_Prefix
{
    /// <summary>
    /// Spoof age gate queries to return adult status.
    /// </summary>
    /// <param name="options">Query options.</param>
    /// <param name="clientData">Client data.</param>
    /// <param name="completionDelegate">Completion callback.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref QueryAgeGateOptions options, object clientData, OnQueryAgeGateCallback completionDelegate)
    {
        if (!AUnlocker.UnlockMinor.Value) return true;

        try
        {
            var spoofedInfo = new QueryAgeGateCallbackInfo
            {
                ResultCode = Result.Success,
                CountryCode = (Utf8String)"US",
                AgeOfConsent = 18
            };

            completionDelegate?.Invoke(ref spoofedInfo);
            return false;
        }
        catch
        {
            return true;
        }
    }
}

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.QueryPermissions))]
public static class QueryPermissions_KWSInterface_QueryPermissions_Prefix
{
    /// <summary>
    /// Spoof permission queries to return adult status.
    /// </summary>
    /// <param name="options">Query options.</param>
    /// <param name="clientData">Client data.</param>
    /// <param name="completionDelegate">Completion callback.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref QueryPermissionsOptions options, object clientData, OnQueryPermissionsCallback completionDelegate)
    {
        if (!AUnlocker.UnlockMinor.Value) return true;

        try
        {
            var spoofedInfo = new QueryPermissionsCallbackInfo
            {
                ResultCode = Result.Success,
                IsMinor = false,
                DateOfBirth = (Utf8String)"1990-01-01",
                ParentEmail = (Utf8String)"verified@parental-consent.com"
            };

            completionDelegate?.Invoke(ref spoofedInfo);
            return false;
        }
        catch
        {
            return true;
        }
    }
}

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.GetPermissionByKey))]
public static class GetPermissionByKey_KWSInterface_GetPermissionByKey_Prefix
{
    /// <summary>
    /// Spoof permission checks to always return granted.
    /// </summary>
    /// <param name="options">Query options.</param>
    /// <param name="outPermission">Output permission status.</param>
    /// <param name="__result">Original return value.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref GetPermissionByKeyOptions options, out KWSPermissionStatus outPermission, ref Result __result)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            outPermission = KWSPermissionStatus.Granted;
            __result = Result.Success;
            return false;
        }
        
        outPermission = default;
        return true;
    }
}

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.CopyPermissionByIndex))]
public static class CopyPermissionByIndex_KWSInterface_CopyPermissionByIndex_Prefix
{
    /// <summary>
    /// Spoof indexed permission lookups to always return granted.
    /// </summary>
    /// <param name="options">Query options.</param>
    /// <param name="outPermission">Output permission status.</param>
    /// <param name="__result">Original return value.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref CopyPermissionByIndexOptions options, out PermissionStatus outPermission, ref Result __result)
    {
        if (AUnlocker.UnlockMinor.Value)
        {
            outPermission = new PermissionStatus
            {
                Status = KWSPermissionStatus.Granted
            };
            __result = Result.Success;
            return false;
        }
        
        outPermission = default;
        return true;
    }
}

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.RequestPermissions))]
public static class RequestPermissions_KWSInterface_RequestPermissions_Prefix
{
    /// <summary>
    /// Spoof permission requests to always return success.
    /// </summary>
    /// <param name="options">Request options.</param>
    /// <param name="clientData">Client data.</param>
    /// <param name="completionDelegate">Completion callback.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref RequestPermissionsOptions options, object clientData, OnRequestPermissionsCallback completionDelegate)
    {
        if (!AUnlocker.UnlockMinor.Value) return true;

        try
        {
            var spoofedInfo = new RequestPermissionsCallbackInfo
            {
                ResultCode = Result.Success
            };

            completionDelegate?.Invoke(ref spoofedInfo);
            return false;
        }
        catch
        {
            return true;
        }
    }
}

[HarmonyPatch(typeof(KWSInterface), nameof(KWSInterface.CreateUser))]
public static class CreateUser_KWSInterface_CreateUser_Prefix
{
    /// <summary>
    /// Spoof user creation to return adult status.
    /// </summary>
    /// <param name="options">Creation options.</param>
    /// <param name="clientData">Client data.</param>
    /// <param name="completionDelegate">Completion callback.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
    {
        if (!AUnlocker.UnlockMinor.Value) return true;

        try
        {
            var spoofedInfo = new CreateUserCallbackInfo
            {
                ResultCode = Result.Success,
                IsMinor = false
            };

            completionDelegate?.Invoke(ref spoofedInfo);
            return false;
        }
        catch
        {
            return true;
        }
    }
}

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

[HarmonyPatch(typeof(PlayerBanData), nameof(PlayerBanData.BanPoints), MethodType.Setter)]
public static class RemoveDisconnectPenalty_PlayerBanData_BanPoints_Prefix
{
    /// <summary>
    /// Remove the time penalty after disconnecting from too many lobbies.
    /// </summary>
    /// <param name="__instance">The <c>PlayerBanData</c> instance.</param>
    /// <param name="value">The value being set to BanPoints.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(PlayerBanData __instance, ref float value)
    {
        if (!AUnlocker.RemovePenalty.Value) return true;
        if (!(bool) (UnityEngine.Object) AmongUsClient.Instance || AmongUsClient.Instance.NetworkMode != NetworkModes.OnlineGame)
            return true;

        value = 0f;
        //__instance.BanPoints = 0f; // Remove all BanPoints
        return false;
    }
}