using BepInEx.Logging;
using Loader.ID;

namespace RF5AutoPickup.Print;

internal static class PrintUpdates
{
    internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("RF5AutoPickup");

    internal static void ShowItemUpdate(ItemDataTable item)
    {
        if (AutoPickup.EnableLogging?.Value == true && item is not null && !string.IsNullOrWhiteSpace(item.ScreenName))
        {
            Log.LogInfo($"{item.ScreenName} has been updated to {(item.IsAutoPickup ? "enable" : "disable")} auto pickup!");
        }
    }

    internal static void PrintException(Exception ex)
    {
        if (AutoPickup.EnableLogging?.Value == true && Log is not null)
        {
            Log.LogError(ex);
        }
    }

    internal static void PrintRemainingItems(Dictionary<ItemID, bool> ItemsToUpdate)
    {
        Log.LogInfo($"Printing list of remaining items.");
        foreach (var item in ItemsToUpdate.Where(x => !x.Value))
        {
            Log.LogInfo($"{item} not yet patched for auto pickup.");
        }
    }

    internal static void PrintRemainingItems(Dictionary<Prefab, bool> ItemsToUpdate)
    {
        Log.LogInfo($"Printing list of remaining items.");
        foreach (var item in ItemsToUpdate.Where(x => !x.Value))
        {
            Log.LogInfo($"{item} not yet patched for auto pickup.");
        }
    }

    internal static void AllItemsPatched()
    {
        Log.LogInfo("Alls items patched for auto pickup.");
    }

    internal static void PrintSettingChanged(object sender, BepInEx.Configuration.SettingChangedEventArgs e)
    {
        Log.LogInfo($"{e.ChangedSetting.Definition} Setting has changed.");
    }
}