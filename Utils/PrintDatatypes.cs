using System.Text;
using BepInEx.Logging;
using DataTable;

#if (NETSTANDARD2_1)
using UnhollowerBaseLib;
#endif

#if (NET6_0)
using Il2CppInterop.Runtime.InteropTypes.Arrays;
#endif

namespace RF5AutoPickup.Print;

internal static class PrintDatatypes
{
    internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("RF5AutoPickup");
    private static bool HasPrintedTable { get; set; } = false;

    internal static void Print(SerializedItemDataTable item)
    {
        if (Log is not null && item is not null)
        {
            Log.LogInfo(SerializedItemDataTableToString(item));
        }
    }

    internal static void Print(ItemDataTable item)
    {
        if (Log is not null && item is not null)
        {
            Log.LogInfo(ItemDataTableToString(item));
        }
    }

    internal static void PrintDatatables(Il2CppArrayBase<SerializedItemDataTable> table)
    {
        if (AutoPickupPlugin.EnableDumpItemDataTablesLogging?.Value == true && !HasPrintedTable && Log is not null && table is not null && table.Count > 100)
        {
            foreach (var item in table)
            {
                Print(item);
            }

            //Only need to run once
            HasPrintedTable = true;
        }
    }

    private static string ItemDataTableToString(ItemDataTable item)
    {
        StringBuilder builder = new StringBuilder();

        return ItemDataTableToString(ref builder, item);
    }

    private static string ItemDataTableToString(ref StringBuilder builder, ItemDataTable item)
    {
        if (builder is null || item is null)
        {
            return string.Empty;
        }

        builder.Append("ItemDataTable");
        builder.Append(" { ");
        if (PrintItemDataTableMembers(ref builder, item))
        {
            builder.Append(' ');
        }

        builder.Append('}');
        return builder.ToString();
    }

    private static bool PrintItemDataTableMembers(ref StringBuilder builder, ItemDataTable item)
    {
        if (builder is null || item is null)
        {
            return false;
        }

        builder.Append("Pointer = ").Append(item.Pointer);
        builder.Append(", CropID = ").Append(item.CropID);
        builder.Append(", IsAutoPickup = ").Append(item.IsAutoPickup);
        builder.Append(", ItemCategory = ").Append(item.ItemCategory);
        builder.Append(", ItemIndex = ").Append(item.ItemIndex);
        builder.Append(", ItemSize = ").Append(item.ItemSize);
        builder.Append(", ItemType = ").Append(item.ItemType);
        builder.Append(", ModelName = ").Append(item.ModelName);
        builder.Append(", ObjectClass = ").Append(item.ObjectClass);
        builder.Append(", OnGroundItemPrefabPath = ").Append(item.OnGroundItemPrefabPath);
        builder.Append(", PrefabID = ").Append(item.PrefabID);
        builder.Append(", ScreenName = ").Append(item.ScreenName);

        return true;
    }

    private static bool PrintSerializedItemDataTableMembers(ref StringBuilder builder, SerializedItemDataTable item)
    {
        if (builder is null || item is null)
        {
            return false;
        }

        builder.Append("Pointer = ").Append(item.Pointer);
        builder.Append(", ID = ").Append(item.ID);
        builder.Append(", ObjectClass = ").Append(item.ObjectClass);
        builder.Append(", Body = ");
        ItemDataTableToString(ref builder, item.Body);
        return true;
    }

    private static string SerializedItemDataTableToString(SerializedItemDataTable item)
    {
        if (item is null)
        {
            return string.Empty;
        }

        StringBuilder builder = new StringBuilder();
        builder.Append("SerializedItemDataTable");
        builder.Append(" { ");
        if (PrintSerializedItemDataTableMembers(ref builder, item))
        {
            builder.Append(' ');
        }

        builder.Append('}');
        return builder.ToString();
    }
}