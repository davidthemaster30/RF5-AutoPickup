using BepInEx.Configuration;
using HarmonyLib;
using RF5AutoPickup.Print;

namespace RF5AutoPickup;

[HarmonyPatch]
internal static class ItemPropertyPatch
{
    internal static readonly Dictionary<ItemID, bool> ItemsToUpdate = new Dictionary<ItemID, bool>();
    internal static bool isFarmArea { get; set; } = false;

    internal static bool isPatched { get; set; } = false;

    //For #577
    [HarmonyPatch(typeof(ItemDataTable), nameof(ItemDataTable.GetDataTable))]
    [HarmonyPostfix]
    internal static void AutoPickupFix(ItemID itemID, ref ItemDataTable __result)
    {
        //result is a ref and models use pointers, so only need to run once
        if (AutoPickupPlugin.IsModDisabled() || isPatched || __result is null)
        {
            return;
        }

        if (KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng.Contains(itemID))
        {
            __result.IsAutoPickup = !isFarmArea && AutoPickupPlugin.EnableAutoPickupGrasses.Value;
        }

        __result.IsAutoPickup = itemID switch
        {
            KnownItemIDs.WitheredGrassItemID => !isFarmArea && AutoPickupPlugin.EnableAutoPickupWitheredGrass.Value,
            KnownItemIDs.RockItemID => !isFarmArea && AutoPickupPlugin.EnableAutoPickupRocks.Value,
            KnownItemIDs.BranchItemID => !isFarmArea && AutoPickupPlugin.EnableAutoPickupBranches.Value,
            KnownItemIDs.CornItemID => isFarmArea && !AutoPickupPlugin.DisableAutoPickupCorn.Value,
            _ => __result.IsAutoPickup
        };

        PrintUpdates.ShowItemAutoPickup(__result);
        TrackUpdate(itemID);

        //force immediate update for next item
        //If not, patch will be called repeatedly and hinder performance
        var item = ItemsToUpdate.FirstOrDefault(x => !x.Value);
        if (item.Key != default)
        {
            _ = ItemDataTable.GetDataTable(item.Key);
        }
    }

    internal static void OnSettingChanged(object sender, EventArgs e)
    {
        var eventArgs = e as SettingChangedEventArgs;
        PrintUpdates.PrintSettingChanged(sender, eventArgs);

        if (ItemDataTable._ItemDataTableArray?.DataTables is null)
        {
            //Change was before data was loaded (on title screen?)
            return;
        }

        Reset();
    }

    internal static void Prepare()
    {
        if (ItemsToUpdate.Count == 0)
        {
            //First pass has items IDs added according to initial config
            foreach (var item in KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng)
            {
                ItemsToUpdate.Add(item, false);
            }

            ItemsToUpdate.Add(KnownItemIDs.WitheredGrassItemID, false);
            ItemsToUpdate.Add(KnownItemIDs.RockItemID, false);
            ItemsToUpdate.Add(KnownItemIDs.BranchItemID, false);
            ItemsToUpdate.Add(KnownItemIDs.CornItemID, false);
        }
    }

    internal static void Reset()
    {
        isPatched = false;

        foreach (var key in ItemsToUpdate.Keys)
        {
            ItemsToUpdate[key] = false;
        }

        Update();
    }

    internal static void Update()
    {
        _ = ItemDataTable.GetDataTable(ItemsToUpdate.First().Key);
    }

    private static void TrackUpdate(ItemID itemID)
    {
        if (ItemsToUpdate.TryGetValue(itemID, out bool val))
        {
            ItemsToUpdate[itemID] = true;
        }

        if (!ItemsToUpdate.Any(x => !x.Value))
        {
            isPatched = true;
            PrintUpdates.AllItemsPatched();
            PrintDatatypes.PrintDatatables(ItemDataTable._ItemDataTableArray.DataTables);
        }
    }
}