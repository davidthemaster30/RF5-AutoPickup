using BepInEx;
using BepInEx.Logging;
using DataTable;
using HarmonyLib;
using Loader.ID;
using RF5AutoPickup.Print;

namespace RF5AutoPickup;

[HarmonyPatch]
internal class ItemPropertyPatch
{
    internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("ItemPropertyPatchLogger");
    internal static readonly Prefab[] PrefabGrassesOnGroundToAutoPickup = [
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0001,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0002,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0006,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0014,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0018,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0021,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0023,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0026,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0037,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0042,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0044,
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0045
    ];

    internal static readonly ItemID[] ItemIDGrassesOnGroundToAutoPickup = [
        ItemID.Item_Grass_0001,
        ItemID.Item_Grass_0002,
        ItemID.Item_Grass_0006,
        ItemID.Item_Grass_0014,
        ItemID.Item_Grass_0018,
        ItemID.Item_Grass_0021,
        ItemID.Item_Grass_0023,
        ItemID.Item_Grass_0026,
        ItemID.Item_Grass_0037,
        ItemID.Item_Grass_0042,
        ItemID.Item_Grass_0044,
        ItemID.Item_Grass_0045
    ];

    internal static bool isPatched { get; set; } = false;
    
    [HarmonyPatch(typeof(ItemDataTable), nameof(ItemDataTable.GetDataTable))]
    [HarmonyPrefix]
    internal static void AutoPickupFix(ItemID itemID)
    {
        //result is a ref and models use pointers, so only need to run once
        if (isPatched)
        {
            return;
        }

        if (ItemDataTable._ItemDataTableArray?.DataTables is not null)
        {
            foreach (var item in ItemDataTable._ItemDataTableArray.DataTables)
            {
                if (item.Body is not null && PrefabGrassesOnGroundToAutoPickup.Contains(item.Body.PrefabID))
                {
                    item.Body.IsAutoPickup = true;
                }
            }

            PrintDatatypes.PrintDatatables(Log, ItemDataTable._ItemDataTableArray.DataTables);

            isPatched = true;
        }
    }
}