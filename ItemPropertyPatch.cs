using BepInEx.Configuration;
using HarmonyLib;
using Loader.ID;
using RF5AutoPickup.Print;

namespace RF5AutoPickup;

[HarmonyPatch]
internal static class ItemPropertyPatch
{
    internal static readonly Dictionary<ItemID, bool> ItemsToUpdate = new Dictionary<ItemID, bool>();

    internal static bool isPatched { get; set; } = false;

    internal static void OnSettingChanged(object sender, EventArgs e)
    {
        var eventArgs = e as SettingChangedEventArgs;
        PrintUpdates.PrintSettingChanged(sender, eventArgs);

        if (ItemDataTable._ItemDataTableArray?.DataTables is null)
        {
            //Change was before data was loaded (on title screen?)
            return;
        }

        isPatched = false;
        //force update toggle on all
        foreach (var key in ItemsToUpdate.Keys)
        {
            ItemsToUpdate[key] = false;
        }

        //force update
        _ = ItemDataTable.GetDataTable(ItemsToUpdate.First().Key);
    }

    internal static void Prepare()
    {
        if (ItemsToUpdate.Count == 0)
        {
            //First pass has items IDs added according to initial config
            foreach (var item in KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng)
            {
                ItemsToUpdate.Add(item, AutoPickup.EnableAutoPickupGrasses.Value);
            }

            ItemsToUpdate.Add(KnownItemIDs.WitheredGrassItemID, AutoPickup.EnableAutoPickupWitheredGrass.Value);
            ItemsToUpdate.Add(KnownItemIDs.RockItemID, AutoPickup.EnableAutoPickupRocks.Value);
            ItemsToUpdate.Add(KnownItemIDs.BranchItemID, AutoPickup.EnableAutoPickupBranches.Value);
            ItemsToUpdate.Add(KnownItemIDs.CornItemID, AutoPickup.DisableAutoPickupCorn.Value);
        }
    }

    //For #577
    [HarmonyPatch(typeof(ItemDataTable), nameof(ItemDataTable.GetDataTable))]
    [HarmonyPostfix]
    internal static void AutoPickupFix(ItemID itemID, ref ItemDataTable __result)
    {
        //result is a ref and models use pointers, so only need to run once
        if (AutoPickup.IsModDisabled() || isPatched || __result is null)
        {
            return;
        }

        if (KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng.Contains(itemID))
        {
            __result.IsAutoPickup = AutoPickup.EnableAutoPickupGrasses.Value;
            PrintUpdates.ShowItemUpdate(__result);
            TrackUpdate(itemID);
        }

        if (itemID == KnownItemIDs.WitheredGrassItemID)
        {
            __result.IsAutoPickup = AutoPickup.EnableAutoPickupWitheredGrass.Value;
            PrintUpdates.ShowItemUpdate(__result);
            TrackUpdate(itemID);
        }

        if (itemID == KnownItemIDs.RockItemID)
        {
            __result.IsAutoPickup = AutoPickup.EnableAutoPickupRocks.Value;
            PrintUpdates.ShowItemUpdate(__result);
            TrackUpdate(itemID);
        }

        if (itemID == KnownItemIDs.BranchItemID)
        {
            __result.IsAutoPickup = AutoPickup.EnableAutoPickupBranches.Value;
            PrintUpdates.ShowItemUpdate(__result);
            TrackUpdate(itemID);
        }

        if (itemID == KnownItemIDs.CornItemID)
        {
            __result.IsAutoPickup = !AutoPickup.DisableAutoPickupCorn.Value;
            PrintUpdates.ShowItemUpdate(__result);
            TrackUpdate(itemID);
        }

        //force immediate update for next item
        //If not, patch will be called repeatedly and hinder performance
        var item = ItemsToUpdate.FirstOrDefault(x => !x.Value);
        if (item.Key != default)
        {
            _ = ItemDataTable.GetDataTable(item.Key);
        }
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