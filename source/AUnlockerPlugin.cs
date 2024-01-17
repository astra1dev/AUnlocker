using AmongUs.Data;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
// using Assets.CoreScripts;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using System.Text;
// using UnityEngine;


namespace AUnlocker;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class AUnlockerPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    { 
        Harmony.PatchAll();
    }


    // ref bool __result stores the return value of the original AU method (e.g. IsFreechatAllowed)
    // By setting __result to true, the original method returns true (-> e.g. enables freechat).
    // In harmony, 'return false' from prefix method -> original method does NOT get executed after prefix


    // Bypass Anticheat kicking you from the lobby if you send long messages
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSendChat))]
    public static class BypassAnticheatAndRatelimit
    {
        public static bool Prefix(PlayerControl __instance, string chatText, ref bool __result)
        {
            // If the player is a client and the HudManager exists, add the chat to the local chat log
            if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance)
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(__instance, chatText);

            // Start an RPC (Remote Procedure Call) to send the chat message to other players. SendOption.None bypasses anticheat
            MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(__instance.NetId, (byte)RpcCalls.SendChat, SendOption.None);
            messageWriter.Write(chatText);
            messageWriter.EndMessage();

            __result = true;
            return false;
        }
    }
}
