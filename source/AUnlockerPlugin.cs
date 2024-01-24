using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

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
}
