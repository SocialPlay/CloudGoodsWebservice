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

        public static event Action<List<StoreItem>> OnInitializedStoreItems;
        public StoreLoader StoreLoader;
        public FilterNewestItems.SortTimeType timeFilterType = FilterNewestItems.SortTimeType.hours;
        public int itemDisplayCount = 0;
        public int timeDifference = 5;

        FilterNewestItems newestItemFilter = new FilterNewestItems();
        List<StoreItem> storeItems = new List<StoreItem>();

        public void InitializeStore()
        {
            ItemStoreServices.GetStoreItems(new StoreItemsRequest(), OnReceivedStoreItems);
        }


        void OnReceivedStoreItems(List<StoreItem> newStoreItems)
        {
            for (int i = 0; i < newStoreItems.Count; i++)
            {
                storeItems.Add(newStoreItems[i]);
            }

            OnInitializedStoreItems(storeItems);

            //StoreLoader.LoadStoreWithPaging(newStoreItems, 0);
        }

    }
}