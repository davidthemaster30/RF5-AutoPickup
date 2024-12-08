using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using BepInEx.Unity.IL2CPP;

namespace RF5AutoPickup;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess(GameProcessName)]
public class AutoPickupPlugin : BasePlugin
{
    private const string PluginConfigSection = "Auto Pickup Changes";
    private const string GameProcessName = "Rune Factory 5.exe";

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

    internal void LoadConfig()
    {
        EnableLogging = Config.Bind("General", nameof(EnableLogging), true, "Set to true to enable logging of changed values by the mod.");

        EnableAutoPickupGrasses = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupGrasses), true, "Set to true to enable auto pickup grasses.");
        EnableAutoPickupGrasses.SettingChanged += ItemPropertyPatch.OnSettingChanged;
        EnableAutoPickupRocks = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupRocks), false, "Set to true to enable auto pickup stone.");
        EnableAutoPickupRocks.SettingChanged += ItemPropertyPatch.OnSettingChanged;
        EnableAutoPickupBranches = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupBranches), false, "Set to true to enable auto pickup lumber.");
        EnableAutoPickupBranches.SettingChanged += ItemPropertyPatch.OnSettingChanged;
        EnableAutoPickupWitheredGrass = Config.Bind(PluginConfigSection, nameof(EnableAutoPickupWitheredGrass), true, "Set to true to enable auto pickup withered grass.");
        EnableAutoPickupWitheredGrass.SettingChanged += ItemPropertyPatch.OnSettingChanged;
        DisableAutoPickupCorn = Config.Bind(PluginConfigSection, nameof(DisableAutoPickupCorn), false, "Set to true to disable auto pickup of corn.");
        DisableAutoPickupCorn.SettingChanged += ItemPropertyPatch.OnSettingChanged;

        EnableDumpItemDataTablesLogging = Config.Bind("Debug", nameof(EnableDumpItemDataTablesLogging), false, "Set to true output itemdatatables to log for mod developpement (NOT RECOMMENDED).");
    }

    public override void Load()
    {
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loading!");

        LoadConfig();
        Harmony.CreateAndPatchAll(typeof(ItemPropertyPatch));

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}