using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using CloudGoods.Utilities;
using CloudGoods.Models;

namespace CloudGoods.Utilities
{
    class UIGridLoader : MonoBehaviour, IGridLoader
    {
        public Transform grid = null;
        public GameObject itemPrefab = null;


        public event Action<PremiumCurrencyBundle, GameObject> ItemAdded;

        public void LoadGrid(List<PremiumCurrencyBundle> PaidCurrenyBundles)
        {
            foreach (PremiumCurrencyBundle PaidCurrencyBundle in PaidCurrenyBundles)
            {
                GameObject gItem = Instantiate(itemPrefab) as GameObject;
                gItem.transform.SetParent(grid, false);
                if (ItemAdded != null)
                    ItemAdded(PaidCurrencyBundle, gItem);
            }
        }
    }
}