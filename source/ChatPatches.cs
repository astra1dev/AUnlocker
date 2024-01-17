using HarmonyLib;
using UnityEngine;

namespace AUnlocker;


// Bypass chat limits (special characters, ctrl+v enabled, etc.)
[HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
public static class BypassChatLimit
{
    public static void Postfix(ChatController __instance)
    {
        if (!__instance.freeChatField.textArea.hasFocus) return;
        // no 3s cooldown when sending messages
        __instance.timeSinceLastMessage = 3f;
        // no character limit
        __instance.freeChatField.textArea.characterLimit = int.MaxValue;
        // allow Ctrl + C and Ctrl + V
        __instance.freeChatField.textArea.AllowPaste = true;
        // allow symbols
        __instance.freeChatField.textArea.AllowSymbols = true;
        // allow emails
        __instance.freeChatField.textArea.AllowEmail = true;

    }
}


// Edit Color indicators for chatbox (only visual)
[HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.UpdateCharCount))]
public static class EditColorIndicators
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
        __instance.allowAllCharacters = true; // not used by game's code, but I include it anyway
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


// Allow copying from the chatbox
[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.Update))]
public static class AllowCopy
{
    public static void Postfix(TextBoxTMP __instance)
    {
        if (!__instance.hasFocus){return;}

        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C))
        {
            ClipboardHelper.PutClipboardString(__instance.text);
        }
    }
}
