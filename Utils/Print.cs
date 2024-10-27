using BepInEx.Logging;
using DataTable;
using System.Text;

namespace RF5AutoPickup.Print;

internal static class PrintDatatypes
{
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
        builder.Append("Pointer = ");
        builder.Append((object)item.Pointer);
        builder.Append(", ID = ");
        builder.Append((object)item.ID);
        builder.Append(", ObjectClass = ");
        builder.Append((object)item.ObjectClass);
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
        builder.Append("Pointer = ");
        builder.Append(item.Pointer);
        builder.Append(", CropID = ");
        builder.Append(item.CropID);
        builder.Append(", IsAutoPickup = ");
        builder.Append(item.IsAutoPickup);
        builder.Append(", ItemCategory = ");
        builder.Append(item.ItemCategory);
        builder.Append(", ItemIndex = ");
        builder.Append(item.ItemIndex);
        builder.Append(", ItemSize = ");
        builder.Append(item.ItemSize);
        builder.Append(", ItemType = ");
        builder.Append(item.ItemType);
        builder.Append(", ModelName = ");
        builder.Append(item.ModelName);
        builder.Append(", ObjectClass = ");
        builder.Append(item.ObjectClass);
        builder.Append(", OnGroundItemPrefabPath = ");
        builder.Append(item.OnGroundItemPrefabPath);
        builder.Append(", PrefabID = ");
        builder.Append(item.PrefabID);
        builder.Append(", ScreenName = ");
        builder.Append(item.ScreenName);

        return true;
    }
}