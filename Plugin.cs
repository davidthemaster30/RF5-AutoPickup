using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

#if (NETSTANDARD2_1)
using BepInEx.IL2CPP;
#endif

#if (NET6_0)
using BepInEx.Unity.IL2CPP;
#endif


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

    internal static void LoadConfig(ConfigFile Config){
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
        // Plugin startup logic
        Log.LogInfo($"Plugin {PluginName} is loaded!");

        // Config
        LoadConfig(Config);

        Harmony.CreateAndPatchAll(typeof(ItemPropertyPatch));
    }
}