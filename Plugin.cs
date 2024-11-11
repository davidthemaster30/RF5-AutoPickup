using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;

using HarmonyLib;

namespace RF5AutoPickup;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInProcess(GameProcessName)]
public class AutoPickup : BasePlugin
{
    #region PluginInfo
    private const string PluginGUID = "RF5AutoPickup";
    private const string PluginName = "RF5AutoPickup";
    private const string PluginConfigSection = "Auto Pickup Changes";
    private const string PluginVersion = "1.1.1";
    private const string GameProcessName = "Rune Factory 5.exe";
    #endregion

    internal static ConfigEntry<bool> EnableAutoPickupGrasses;
    internal static ConfigEntry<bool> EnableAutoPickupRocks;
    internal static ConfigEntry<bool> EnableAutoPickupBranches;
    internal static ConfigEntry<bool> EnableAutoPickupWitheredGrass;
    internal static ConfigEntry<bool> DisableAutoPickupCorn;

    internal static ConfigEntry<bool> EnableLogging;
    internal static ConfigEntry<bool> EnableDumpItemDataTablesLogging;

    internal static bool IsModDisabled()
    {
        return EnableAutoPickupGrasses is not null && EnableAutoPickupGrasses.Value &&
        EnableAutoPickupRocks is not null && EnableAutoPickupRocks.Value &&
        EnableAutoPickupBranches is not null && EnableAutoPickupBranches.Value &&
        EnableAutoPickupWitheredGrass is not null && EnableAutoPickupWitheredGrass.Value &&
        DisableAutoPickupCorn is not null && DisableAutoPickupCorn.Value;
    }

    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {PluginName} is loaded!");

        // Config
        EnableLogging = Config.Bind("General", nameof(EnableLogging), true, "Set to true to enable logging of changed values by the mod.");

        EnableAutoPickupGrasses = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupGrasses), true, "Set to true to enable auto pickup grasses.");
        EnableAutoPickupRocks = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupRocks), false, "Set to true to enable auto pickup stone.");
        EnableAutoPickupBranches = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupBranches), false, "Set to true to enable auto pickup lumber.");
        EnableAutoPickupWitheredGrass = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupWitheredGrass), true, "Set to true to enable auto pickup withered grass.");
        DisableAutoPickupCorn = Config.Bind(PluginConfigSection, nameof(DisableAutoPickupCorn), false, "Set to true to disable auto pickup of corn.");

        EnableDumpItemDataTablesLogging = Config.Bind("Debug", nameof(EnableDumpItemDataTablesLogging), false, "Set to true output itemdatatables to log for mod developpement (NOT RECOMMENDED).");


        Harmony.CreateAndPatchAll(typeof(ItemPropertyPatch));
    }
}