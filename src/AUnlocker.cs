using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BepInEx.Configuration;

namespace AUnlocker;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
public partial class AUnlocker : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static ConfigEntry<bool> PatchAccount;
    public static ConfigEntry<bool> PatchChat;
    public static ConfigEntry<bool> PatchCosmetics;

    public override void Load()
    { 
        PatchAccount = Config.Bind("Patches", "AccountPatches", true, "Enable account-related patches");
        PatchChat = Config.Bind("Patches", "ChatPatches", true, "Enable chat-related patches");
        PatchCosmetics = Config.Bind("Patches", "CosmeticPatches", true, "Enable cosmetic-related patches");
        Harmony.PatchAll();
    }
}
