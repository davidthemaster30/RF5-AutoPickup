using BepInEx.Logging;

namespace RF5AutoPickup.Print;

internal static class PrintUpdates
{
    internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("ItemPropertyPatchLogger");

    internal static void ShowItemUpdate(ItemDataTable item)
    {
        if (AutoPickup.EnableLogging?.Value == true && Log is not null && item is not null && !string.IsNullOrWhiteSpace(item.ScreenName))
        {
            Log.LogInfo($"{item.ScreenName} has been updated to {(item.IsAutoPickup ? "enable" : "disable")} auto pickup!");
        }
    }
}