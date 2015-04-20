using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using System;
using CloudGoods.Services;


namespace CloudGoods.SDK.Store
{
    public class StoreInitializer : MonoBehaviour
    {
        public StoreLoader StoreLoader;
        public FilterNewestItems.SortTimeType timeFilterType = FilterNewestItems.SortTimeType.hours;
        public int itemDisplayCount = 0;
        public int timeDifference = 5;

        FilterNewestItems newestItemFilter = new FilterNewestItems();
        List<StoreItem> storeItems = new List<StoreItem>();

        public void InitializeStore()
        {
            ItemStoreServices.GetStoreItems(OnReceivedStoreItems);
        }


        void OnReceivedStoreItems(List<StoreItem> newStoreItems)
        {
            for (int i = 0; i < newStoreItems.Count; i++)
            {
                storeItems.Add(newStoreItems[i]);
            }

            StoreLoader.LoadStoreWithPaging(newStoreItems, 0);
        }

    }
}