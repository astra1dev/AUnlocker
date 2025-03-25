using HarmonyLib;
using UnityEngine;

namespace AUnlocker;

// ChatJailbreak
[HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
public static class ChatJailbreak_ChatController_Update_Postfix
{
    public static void Postfix(ChatController __instance)
    {
        // [UNSAFE] No Chat Cooldown
        if (AUnlocker.NoChatCooldown.Value)
        {
            // if (!__instance.freeChatField.textArea.hasFocus) return;
            __instance.timeSinceLastMessage = 3f;
        }

        // [UNSAFE] No Character Limit
        if (AUnlocker.NoCharacterLimit.Value)
        {
            __instance.freeChatField.textArea.characterLimit = int.MaxValue;
        }

        else if (AUnlocker.PatchChat.Value)
        {
            __instance.freeChatField.textArea.AllowPaste = true;
            __instance.freeChatField.textArea.AllowSymbols = true;
            __instance.freeChatField.textArea.AllowEmail = true;
            __instance.freeChatField.textArea.allowAllCharacters = true;
            __instance.freeChatField.textArea.characterLimit = 120;  // above 120 characters anticheat will kick you
        }
    }
}

// Edit Color indicators for chatbox (only visual)
[HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.UpdateCharCount))]
public static class EditColorIndicators_FreeChatInputField_UpdateCharCount_Postfix
{
    public static void Postfix(FreeChatInputField __instance)
    {
        if (AUnlocker.NoCharacterLimit.Value)
        {
            int length = __instance.textArea.text.Length;
            // Show new character limit below text field
            __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");

            if (length < 1610612735) // Black if not close to limit (under 75%)
                __instance.charCountText.color = Color.black;
            else if (length < 2147483647) // Yellow if close to limit (under 100%)
                __instance.charCountText.color = new Color(1f, 1f, 0f, 1f);
            else // Red if limit reached (equal or over 100%)
                __instance.charCountText.color = Color.red;
        }

        else if (AUnlocker.PatchChat.Value)
        {
            int length = __instance.textArea.text.Length;
            // Show new character limit below text field
            __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");

            if (length < 90) // Black if not close to limit (under 75%)
                __instance.charCountText.color = Color.black;
            else if (length < 120) // Yellow if close to limit (under 100%)
                __instance.charCountText.color = new Color(1f, 1f, 0f, 1f);
            else // Red if limit reached (equal or over 100%)
                __instance.charCountText.color = Color.red;
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
        return true;
    }
}

// [UNSAFE] Allow any characters
[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.IsCharAllowed))]
public static class AllowAllCharacters_TextBoxTMP_IsCharAllowed_Prefix
{
    public static bool Prefix(TextBoxTMP __instance, char i, ref bool __result)
    {
        if (AUnlocker.AllowAllCharacters.Value)
        {
            // Bugfix: chinese characters and others (see issue #31)
            if (i >= 'À' && i <= 'ÿ')
            {
                __result = true;
                return false;
            }
            if (i >= 'Ѐ' && i <= 'џ')
            {
                __result = true;
                return false;
            }
            if (i >= '\u3040' && i <= '㆟')
            {
                __result = true;
                return false;
            }
            if (i >= 'ⱡ' && i <= '힣')
            {
                __result = true;
                return false;
            }
            if (TextBoxTMP.SymbolChars.Contains(i))
            {
                __result = true;
                return false;
            }
            if (TextBoxTMP.EmailChars.Contains(i))
            {
                __result = true;
                return false;
            }
            // Bugfix: backspace messing with chat message;
            // newline / "enter" to prevent message sending "randomly" (see issue #25)
            if (i == '\b' || i == '\n' || i == '\r')
            {
                __result = false;
                return false;
            }

            // // logging
            // string charRepresentation = i switch
            // {
            //     '\b' => "\\b",
            //     '\n' => "\\n",
            //     '\r' => "\\r",
            //     _ => i.ToString()
            // };

            // Debug.Log($"IsCharAllowed({charRepresentation}) (Unicode: {(int)i}) = {__result}");

            // accept any other character by default (including emojis, special characters, etc.)
            // this can cause issues where we would have to deny certain characters 
            // that are messing with the chatbox (like we saw in issues #25 and #31)
            __result = true;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.Start))]
public static class AllowAllCharacters_TextBoxTMP_Start_Postfix
{
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
