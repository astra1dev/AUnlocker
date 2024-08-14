using HarmonyLib;
using UnityEngine;

namespace AUnlocker;


// ChatJailbreak
[HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
public static class ChatJailbreak_ChatController_Update_Postfix
{
    public static void Postfix(ChatController __instance)
    {
        if (AUnlocker.PatchChat.Value)
        { 
            if (!__instance.freeChatField.textArea.hasFocus) return;
            __instance.freeChatField.textArea.AllowPaste = true;
            __instance.freeChatField.textArea.AllowSymbols = true;
            __instance.freeChatField.textArea.AllowEmail = true;
        }
    }
}

// Allow URLs in messages
[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendFreeChat))]
public static class AllowURLS_ChatController_SendFreeChat_Prefix
{
    public static bool Prefix(ChatController __instance)
    {
        if (AUnlocker.PatchChat.Value)
        { 
            string text = __instance.freeChatField.Text;
            ChatController.Logger.Debug("SendFreeChat () :: Sending message: '" + text + "'", null);
            PlayerControl.LocalPlayer.RpcSendChat(text);
            return false;
        }
        else return true;
    }
}

// Allow special characters (beware: some get you kicked by anti-cheat)
[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.IsCharAllowed))]
public static class AllowAllCharacters_TextBoxTMP_IsCharAllowed_Prefix
{
    public static bool Prefix(TextBoxTMP __instance, char i, ref bool __result)
    {
        if (AUnlocker.PatchChat.Value)
        { 
            __result = !(i == '\b');    // Bugfix: '\b' messing with chat message
            return false;
        }
        else return true;
    }

    public static void Postfix(TextBoxTMP __instance)
    {
        if (AUnlocker.PatchChat.Value)
        { 
            __instance.allowAllCharacters = true; // not used by game's code, but I include it anyway
            __instance.AllowEmail = true; 
            __instance.AllowPaste = true;
            __instance.AllowSymbols = true;
        }
    }
}

// Allow copying from the chatbox
[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.Update))]
public static class AllowCopy_TextBoxTMP_Update_Postfix
{
    public static void Postfix(TextBoxTMP __instance)
    {
        if (AUnlocker.PatchChat.Value)
        { 
            if (!__instance.hasFocus){return;}

            // If the user is pressing Ctrl + C, copy the text from the chatbox to the device's clipboard
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C))
            {
                ClipboardHelper.PutClipboardString(__instance.text);
            }
        }
    }
}
