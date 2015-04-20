using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Services;

namespace CloudGoods.SDK.Item
{
    public class ItemPrefabInitilizer
    {

        public static GameObject GetPrefabToInstantiate(ItemInformation itemData, GameObject defaultPrefab = null)
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

        static bool IsPrefabForItem(ItemInformation itemData, DropPrefab dropPrefab)
        {
            return false;
        }

        [System.Serializable]
        public class DropPrefab
        {
            public GameObject prefab;
        }

    }
}




