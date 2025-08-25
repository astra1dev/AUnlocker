using HarmonyLib;
using UnityEngine;
using static AUnlocker.ChatHistory_ChatController_SendChat_Prefix;

namespace AUnlocker;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
public static class ChatJailbreak_ChatController_Update_Postfix
{
    public static AUnlocker Plugin { get; internal set; }

    // CurrentHistorySelection: -1 = no selection, 0 = first message, Count - 1 = last message
    public static int CurrentHistorySelection = -1;
    private static string inProgressMessage = "";
    private static bool isNavigatingHistory = false;

    /// <summary>
    /// Remove the chat cooldown and the character limit. Add the ability to scroll through previous chat messages using the up and down arrow keys.
    /// </summary>
    /// <param name="__instance">The <c>ChatController</c> instance.</param>
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
            //__instance.freeChatField.textArea.AllowPaste = true;
            __instance.freeChatField.textArea.AllowSymbols = true;
            __instance.freeChatField.textArea.AllowEmail = true;
            __instance.freeChatField.textArea.allowAllCharacters = true;
            __instance.freeChatField.textArea.characterLimit = 120;  // above 120 characters anti-cheat will kick you
        }

        // User is trying to navigate up the chat history
        if (Input.GetKeyDown(KeyCode.UpArrow) && ChatHistory.Count > 0)
        {
            if (!isNavigatingHistory)
            {
                // Store the in-progress text so we can restore it later
                inProgressMessage = __instance.freeChatField.textArea.text;
                isNavigatingHistory = true;
            }

            if (CurrentHistorySelection == 0)
            {
                SoundManager.Instance.PlaySound(__instance.warningSound, false);
                Plugin.Log.LogInfo("You have reached the end of your chat history.");
            }
            else
            {
                // Ensure the index (current selection) is within bounds of the ChatHistory list (0 to Count - 1)
                CurrentHistorySelection = Mathf.Clamp(--CurrentHistorySelection, 0, ChatHistory.Count - 1);
                __instance.freeChatField.textArea.SetText(ChatHistory[CurrentHistorySelection]);
            }
        }

        // User is trying to navigate down the chat history
        if (Input.GetKeyDown(KeyCode.DownArrow) && ChatHistory.Count > 0)
        {
            CurrentHistorySelection++;
            if (CurrentHistorySelection < ChatHistory.Count)
            {
                __instance.freeChatField.textArea.SetText(ChatHistory[CurrentHistorySelection]);
            }
            // User has navigated past the most recent message, restore the in-progress text
            else
            {
                __instance.freeChatField.textArea.SetText(inProgressMessage);
                isNavigatingHistory = false;
            }
        }
    }
}

[HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.UpdateCharCount))]
public static class EditColorIndicators_FreeChatInputField_UpdateCharCount_Postfix
{
    /// <summary>
    /// Update the character count color indicator based on the current text length.
    /// </summary>
    /// <param name="__instance">The <c>FreeChatInputField</c> instance.</param>
    public static void Postfix(FreeChatInputField __instance)
    {
        if (AUnlocker.NoCharacterLimit.Value)
        {
            var length = __instance.textArea.text.Length;
            // Show new character limit below text field
            __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");

            __instance.charCountText.color = length switch
            {
                // Black if not close to limit (under 75%)
                < 1610612735 => Color.black,
                // Yellow if close to limit (under 100%)
                < 2147483647 => new Color(1f, 1f, 0f, 1f),
                _ => Color.red
            };
        }

        else if (AUnlocker.PatchChat.Value)
        {
            var length = __instance.textArea.text.Length;
            // Show new character limit below text field
            __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");

            __instance.charCountText.color = length switch
            {
                // Black if not close to limit (under 75%)
                < 90 => Color.black,
                // Yellow if close to limit (under 100%)
                < 120 => new Color(1f, 1f, 0f, 1f),
                _ => Color.red
            };
        }
    }
}

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendFreeChat))]
public static class AllowURLS_ChatController_SendFreeChat_Prefix
{
    /// <summary>
    /// Remove the URL filtering when sending a chat message.
    /// </summary>
    /// <param name="__instance">The <c>ChatController</c> instance.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ChatController __instance)
    {
        if (!AUnlocker.PatchChat.Value) return true;

        var text = __instance.freeChatField.Text;
        ChatController.Logger.Debug($"SendFreeChat() :: Sending message: '{text}'");
        PlayerControl.LocalPlayer.RpcSendChat(text);
        return false;
    }
}

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
public static class ChatHistory_ChatController_SendChat_Prefix
{
    // ChatHistory: index 0 = earliest message, highest index = latest message
    public static readonly List<string> ChatHistory = [];

    /// <summary>
    /// When sending a chat message (either via pressing enter or clicking the send button), add it to the chat history.
    /// </summary>
    /// <param name="__instance">The <c>ChatController</c> instance.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(ChatController __instance)
    {
        var text = __instance.freeChatField.textArea.text;
        // Add to chat history if empty or not the same as the previous message
        // Also this intentionally allows empty / whitespace-only messages to be added to history
        if (ChatHistory.LastOrDefault() != text)
            ChatHistory.Add(text);
        ChatJailbreak_ChatController_Update_Postfix.CurrentHistorySelection = ChatHistory.Count;
        return true;
    }
}

[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.IsCharAllowed))]
public static class AllowAllCharacters_TextBoxTMP_IsCharAllowed_Prefix
{
    /// <summary>
    /// Allow any character to be typed into the chatbox.
    /// </summary>
    /// <param name="__instance">The <c>TextBoxTMP</c> instance.</param>
    /// <param name="__result">Original return value of <c>IsCharAllowed</c>.</param>
    /// <param name="i"> The character to check.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static bool Prefix(TextBoxTMP __instance, ref bool __result, char i)
    {
        // Original game code:
        // public bool IsCharAllowed(char i)
        // {
        //   return this.IpMode ? i >= '0' && i <= '9' || i == '.' : i == ' ' || i >= 'A' && i <= 'Z' || i >= 'a' && i <= 'z' || i >= '0' && i <= '9' || i >= 'À' && i <= 'ÿ' || i >= 'Ѐ' && i <= 'џ' || i >= '\u3040' && i <= '㆟' || i >= 'ⱡ' && i <= '힣' || this.AllowSymbols && TextBoxTMP.SymbolChars.Contains(i) || this.AllowEmail && TextBoxTMP.EmailChars.Contains(i);
        // }

        if (!AUnlocker.AllowAllCharacters.Value) return true;

        if (i is >= 'À' and <= 'ÿ')
        {
            __result = true;
            return false;
        }
        if (i is >= 'Ѐ' and <= 'џ')
        {
            __result = true;
            return false;
        }
        if (i is >= '\u3040' and <= '㆟')
        {
            __result = true;
            return false;
        }
        if (i is >= 'ⱡ' and <= '힣')
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
        if (i is '\b' or '\n' or '\r')
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
}

[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.Start))]
public static class AllowPaste_TextBoxTMP_Start_Postfix
{
    /// <summary>
    /// Allow email symbols to be typed into the chatbox and enables pasting text with CTRL + V.
    /// </summary>
    /// <param name="__instance">The <c>TextBoxTMP</c> instance.</param>
    public static void Postfix(TextBoxTMP __instance)
    {
        if (!AUnlocker.PatchChat.Value) return;

        __instance.allowAllCharacters = true; // not used by game's code, but I include it anyway
        __instance.AllowEmail = true;
        //__instance.AllowPaste = true;
        __instance.AllowSymbols = true;
    }
}

[HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
public static class ChatHistoryLimit_ChatController_Awake_Postfix
{
    /// <summary>
    /// Modify the maximum amount of chat messages to keep in the chat history.
    /// </summary>
    /// <param name="__instance">The <c>ChatController</c> instance.</param>
    /// <returns><c>false</c> to skip the original method, <c>true</c> to allow the original method to run.</returns>
    public static void Postfix(ChatController __instance)
    {
        __instance.chatBubblePool.poolSize = AUnlocker.ChatHistoryLimit.Value;
        // Call ReclaimOldest so the pool is re-initialized with our new size
        __instance.chatBubblePool.ReclaimOldest();
    }
}

[HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.Update))]
public static class AllowCopy_TextBoxTMP_Update_Postfix
{
    /// <summary>
    /// Allow copying, pasting and cutting text between the chatbox and the device's clipboard.
    /// </summary>
    /// <param name="__instance">The <c>TextBoxTMP</c> instance.</param>
    public static void Postfix(TextBoxTMP __instance)
    {
        if (!AUnlocker.PatchChat.Value || !__instance.hasFocus) return;

        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            // ClipboardHelper.PutClipboardString(__instance.text);
            GUIUtility.systemCopyBuffer = __instance.text;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            __instance.SetText(__instance.text + GUIUtility.systemCopyBuffer);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // ClipboardHelper.PutClipboardString(__instance.text);
            GUIUtility.systemCopyBuffer = __instance.text;
            __instance.SetText("");
        }
    }
}
