using AmongUs.Data;
using Assets.CoreScripts;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;


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


    // Bypass chat limits (special characters, ctrl+v enabled, etc.)
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
    public static class BypassChatLimit
    {
        public static void Postfix(ChatController __instance)
        {
            if (!__instance.freeChatField.textArea.hasFocus) return;
            // no 3s cooldown when sending messages
            __instance.timeSinceLastMessage = 3f;
            // if player is host, set limit to 999, otherwise set it to 300 because you might get kicked by server
            __instance.freeChatField.textArea.characterLimit = AmongUsClient.Instance.AmHost ? 999 : 300;
            // allow Ctrl + C and Ctrl + V
            __instance.freeChatField.textArea.AllowPaste = true;
            
        }
    }


    // Edit Color indicators for chatbox (only visual)
    [HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.UpdateCharCount))]
    internal class EditColorIndicators
    {
        public static void Postfix(FreeChatInputField __instance)
        {
            int length = __instance.textArea.text.Length;
            // Show new character limit of 300 / 999 instead of vanilla 100 below text field
            __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");
            if (length < (AmongUsClient.Instance.AmHost ? 888 : 250))
                // Black if not close to limit
                __instance.charCountText.color = Color.black;
            else if (length < (AmongUsClient.Instance.AmHost ? 999 : 300))
                // Yellow if close to limit
                __instance.charCountText.color = new Color(1f, 1f, 0f, 1f);
            else
                // Red if limit reached
                __instance.charCountText.color = Color.red;
        }
    }


    // Bypass Anticheat kicking you from the lobby if you send long messages
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSendChat))]
    public static class BypassAnticheatAndRatelimit
    {
        public static bool Prefix(PlayerControl __instance, string chatText, ref bool __result)
        {
            // If the player is a client and the HudManager exists, add the chat to the local chat log
            if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance)
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(__instance, chatText);

            // Start an RPC (Remote Procedure Call) to send the chat message to other players
            MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(__instance.NetId, (byte)RpcCalls.SendChat, SendOption.None);
            messageWriter.Write(chatText);
            messageWriter.EndMessage();

            __result = true;
            return false;
        }
    }


    // Allow URLs in messages
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendFreeChat))]
    public static class AllowURLS
    {
        public static bool Prefix(ChatController __instance)
        {
            // Remove deletion of any URLs
            string text = __instance.freeChatField.Text;
            ChatController.Logger.Debug("SendFreeChat () :: Sending message: '" + text + "'", null);
            PlayerControl.LocalPlayer.RpcSendChat(text);
            return false;
        }
    }


    // Allow special characters
    [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.IsCharAllowed))]
    public static class AllowAllCharacters
    {
        public static bool Prefix(TextBoxTMP __instance, ref bool __result)
        {
            // Set IsCharAllowed to always return true => every character is allowed
            __result = true;
            return false;
        }

        public static void Postfix(TextBoxTMP __instance)
        {
            __instance.allowAllCharacters = true; // not used by game's code, but I still including anyway :)
            __instance.AllowEmail = true; // self-explanatory
            __instance.AllowPaste = true;
            __instance.AllowSymbols = true;
        }

        // Fix weird backspace bug that occurs when allowing any symbol
        public static bool Prefix(TextBoxTMP __instance, char i, ref bool __result)
        {
            __result = !(i == '\b');
            return false;
        }
    }
}
