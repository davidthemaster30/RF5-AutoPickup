using HarmonyLib;
using Loader.ID;
using RF5AutoPickup.Print;

namespace RF5AutoPickup;

[HarmonyPatch]
internal static class ItemPropertyPatch
{
    internal static readonly Dictionary<ItemID, bool> ItemsToUpdate = new Dictionary<ItemID, bool>();

    internal static readonly Dictionary<Prefab, bool> PrefabsToUpdate = new Dictionary<Prefab, bool>();

    internal static bool isPatched { get; set; } = false;

    internal static void Prepare()
    {
        if (ItemsToUpdate.Count == 0)
        {
            if (AutoPickup.EnableAutoPickupGrasses.Value)
            {
                foreach (var item in KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng)
                {
                    ItemsToUpdate.Add(item, false);
                }
            }

            if (AutoPickup.EnableAutoPickupWitheredGrass.Value)
            {
                ItemsToUpdate.Add(KnownItemIDs.WitheredGrassItemID, false);
            }

            if (AutoPickup.EnableAutoPickupRocks.Value)
            {
                ItemsToUpdate.Add(KnownItemIDs.RockItemID, false);
            }

            if (AutoPickup.EnableAutoPickupBranches.Value)
            {
                ItemsToUpdate.Add(KnownItemIDs.BranchItemID, false);
            }

            if (AutoPickup.DisableAutoPickupCorn.Value)
            {
                ItemsToUpdate.Add(KnownItemIDs.CornItemID, false);
            }
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

        if (AutoPickup.EnableAutoPickupGrasses.Value && KnownItemIDs.ItemIDGrassesOnGroundToAutoPickupEng.Contains(itemID))
        {
            TrackUpdate(itemID);
            __result.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(__result);
        }

        if (AutoPickup.EnableAutoPickupWitheredGrass.Value && itemID == KnownItemIDs.WitheredGrassItemID)
        {
            TrackUpdate(itemID);
            __result.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(__result);
        }

        if (AutoPickup.EnableAutoPickupRocks.Value && itemID == KnownItemIDs.RockItemID)
        {
            TrackUpdate(itemID);
            __result.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(__result);
        }

        if (AutoPickup.EnableAutoPickupBranches.Value && itemID == KnownItemIDs.BranchItemID)
        {
            TrackUpdate(itemID);
            __result.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(__result);
        }

        if (AutoPickup.DisableAutoPickupCorn.Value && itemID == KnownItemIDs.CornItemID)
        {
            TrackUpdate(itemID);
            __result.IsAutoPickup = false;
            PrintUpdates.ShowItemUpdate(__result);
        }

        //force immediate update for next item
        //If not, patch will be called repeatedly and hinder performance
        var item = ItemsToUpdate.FirstOrDefault(x => !x.Value);
        if (item.Key != default)
        {
            PrintUpdates.Log.LogMessage($"force update of {item}");
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
        }
    }
}