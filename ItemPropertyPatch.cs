using HarmonyLib;
using Loader.ID;
using RF5AutoPickup.Print;

namespace RF5AutoPickup;

[HarmonyPatch]
internal sealed class ItemPropertyPatch
{
    internal static readonly Prefab[] PrefabGrassesOnGroundToAutoPickup = [
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0001, //Indigo Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0002, //Blue Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0006, //Elli Leaves
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0014, //Yellow Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0018, //Black Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0021, //Weeds
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0023, //White Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0026, //Orange Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0037, //Antidote Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0042, //Green Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0044, //Purple Grass
        Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0045  //Medicinal Herb
    ];

    internal static readonly Prefab WitheredGrassPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0013; //Withered Grass
    internal static readonly Prefab RockPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_COLLECTION_0175; //Rock
    internal static readonly Prefab BranchPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_COLLECTION_0176; //Branch
    internal static readonly Prefab CornPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_CROP_0034; //Corn

    internal static bool isPatched { get; set; } = false;

    [HarmonyPatch(typeof(ItemDataTable), nameof(ItemDataTable.GetDataTable))]
    [HarmonyPrefix]
    internal static void AutoPickupFix(ItemID itemID)
    {
        //result is a ref and models use pointers, so only need to run once
        if (AutoPickup.IsModDisabled() || isPatched)
        {
            return;
        }

        //Wait until datatables is populated before updating
        if (ItemDataTable._ItemDataTableArray?.DataTables is not null)
        {
            AddAutoPickupGrasses();
            AddAutoPickupWitheredGrass();
            AddAutoPickupRocks();
            AddAutoPickupBranches();
            RemoveAutoPickupCorn();

            PrintDatatypes.PrintDatatables(ItemDataTable._ItemDataTableArray.DataTables);

            isPatched = true;
        }
    }

    internal static void AddAutoPickupGrasses()
    {
        if (AutoPickup.EnableAutoPickupGrasses?.Value == true)
        {
            var grasses = ItemDataTable._ItemDataTableArray.DataTables.Select(item => item.Body).Where(body => body is not null && PrefabGrassesOnGroundToAutoPickup.Contains(body.PrefabID));
            foreach (var grass in grasses)
            {
                grass.IsAutoPickup = true;
                PrintUpdates.ShowItemUpdate(grass);
            }
        }
    }

    internal static void AddAutoPickupWitheredGrass()
    {
        if (AutoPickup.EnableAutoPickupWitheredGrass?.Value == true)
        {
            var witheredGrass = ItemDataTable._ItemDataTableArray.DataTables.Select(item => item.Body).Single(body => body is not null && body.PrefabID == WitheredGrassPrefab);
            witheredGrass.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(witheredGrass);
        }
    }

    internal static void AddAutoPickupRocks()
    {
        if (AutoPickup.EnableAutoPickupRocks?.Value == true)
        {
            var rock = ItemDataTable._ItemDataTableArray.DataTables.Select(item => item.Body).Single(body => body is not null && body.PrefabID == RockPrefab);
            rock.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(rock);
        }
    }

    internal static void AddAutoPickupBranches()
    {
        if (AutoPickup.EnableAutoPickupBranches?.Value == true)
        {
            var branch = ItemDataTable._ItemDataTableArray.DataTables.Select(item => item.Body).Single(body => body is not null && body.PrefabID == BranchPrefab);
            branch.IsAutoPickup = true;
            PrintUpdates.ShowItemUpdate(branch);
        }
    }

    internal static void RemoveAutoPickupCorn()
    {
        if (AutoPickup.DisableAutoPickupCorn?.Value == true)
        {
            var corn = ItemDataTable._ItemDataTableArray.DataTables.Select(item => item.Body).Single(body => body is not null && body.PrefabID == CornPrefab);
            corn.IsAutoPickup = false;
            PrintUpdates.ShowItemUpdate(corn);
        }
    }
}