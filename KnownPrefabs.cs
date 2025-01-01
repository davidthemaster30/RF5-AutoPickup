using Loader.ID;

namespace RF5AutoPickup;

internal static class KnownPrefabs
{
    internal const Prefab BranchPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_COLLECTION_0176; //Branch
    internal const Prefab CornPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_CROP_0034; //Corn
    internal const Prefab RockPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_COLLECTION_0175; //Rock

    internal const Prefab WitheredGrassPrefab = Prefab.ITEM_ONGROUNDITEM_ONGROUNDITEM_ITEM_GRASS_0013; //Withered Grass
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
}