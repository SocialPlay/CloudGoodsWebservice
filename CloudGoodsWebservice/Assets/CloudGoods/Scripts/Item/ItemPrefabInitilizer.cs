using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemPrefabInitilizer
{

    public static GameObject GetPrefabToInstantiate(ItemData itemData, GameObject defaultPrefab = null)
    {
        var prefab = (defaultPrefab != null ? defaultPrefab : CloudGoodsSettings.DefaultItemDrop);
        foreach (var dropPrefab in CloudGoodsSettings.ExtraItemPrefabs)

        {
            if (IsPrefabForItem(itemData, dropPrefab))
            {
                prefab = dropPrefab.prefab;
            }
        }
        return prefab;
    }

    static bool IsPrefabForItem(ItemData itemData, DropPrefab dropPrefab)
    {
        return false;
    }

    [System.Serializable]
    public class DropPrefab
    {
        public GameObject prefab;
    }

}




