using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;

using HarmonyLib;

namespace RF5AutoPickup;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInProcess(GAME_PROCESS)]
public class AutoPickup : BasePlugin
{
    #region PluginInfo
    private const string PLUGIN_GUID = "RF5AutoPickup";
    private const string PLUGIN_NAME = "RF5AutoPickup";
    private const string PLUGIN_VERSION = "1.0.0";
    private const string GAME_PROCESS = "Rune Factory 5.exe";
    #endregion
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {PLUGIN_NAME} is loaded!");

        Harmony.CreateAndPatchAll(typeof(ItemPropertyPatch));
    }
}