using UnityEngine;

namespace AUnlocker.Components;

public class KeybindListener : MonoBehaviour
{
    public AUnlocker Plugin { get; internal set; }

    public void Update()
    {
        if (!Input.GetKeyDown(AUnlocker.ReloadConfigKeybind.Value)) return;
        Plugin.Config.Reload();
        AUnlocker.Log.LogInfo("Configuration reloaded.");
    }
}
