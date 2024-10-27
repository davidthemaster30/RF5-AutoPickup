using BepInEx.Logging;
using DataTable;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Text;

namespace RF5AutoPickup.Print;

internal static class PrintDatatypes
{
    internal static bool EnableLogging { get; set; } = false;

    internal static void PrintDatatables(ManualLogSource Log, Il2CppArrayBase<SerializedItemDataTable> table)
    {
        if (EnableLogging && Log is not null && table is not null)
        {
            foreach (var item in table)
            {
                Print(Log, item);
            }
        }
    }

    internal static void Print(ManualLogSource Log, SerializedItemDataTable item)
    {
        if (Log is not null && item is not null)
        {
            Log.LogInfo(SerializedItemDataTableToString(item));
        }
    }

    internal static void Print(ManualLogSource Log, ItemDataTable item)
    {
        if (Log is not null && item is not null)
        {
            Log.LogInfo(ItemDataTableToString(item));
        }
    }

    internal static string SerializedItemDataTableToString(SerializedItemDataTable item)
    {
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

    internal static bool PrintSerializedItemDataTableMembers(ref StringBuilder builder, SerializedItemDataTable item)
    {
        builder.Append("Pointer = ").Append(item.Pointer);
        builder.Append(", ID = ").Append(item.ID);
        builder.Append(", ObjectClass = ").Append(item.ObjectClass);
        builder.Append(", Body = ");
        ItemDataTableToString(ref builder, item.Body);
        return true;
    }

    internal static string ItemDataTableToString(ItemDataTable item)
    {
        StringBuilder builder = new StringBuilder();

        return ItemDataTableToString(ref builder, item);
    }

    internal static string ItemDataTableToString(ref StringBuilder builder, ItemDataTable item)
    {
        if (item is null)
        {
            return String.Empty;
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

    internal static bool PrintItemDataTableMembers(ref StringBuilder builder, ItemDataTable item)
    {
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
}