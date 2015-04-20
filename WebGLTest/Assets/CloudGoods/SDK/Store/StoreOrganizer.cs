using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Store;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Store
{
    public class StoreOrganizer : MonoBehaviour
    {
        public StoreLoader storeLoader;
        public InputValueChange SearchInput;
        private ISortItem currentSort;
        private int currentSortDirection = 1;


        void SortStoreItemsBy_SortUpdate(ISortItem CurrentSort, int direction)
        {
            currentSort = CurrentSort;
            currentSortDirection = direction;
            OrganizeStore();
        }


        void OrganizeStore()
        {
            List<StoreItem> AllItems = storeLoader.GetStoreItemList();
            if (AllItems.Count == 0)
            {
                Debug.Log("No items to sort at this point");
                return;
            }
            List<StoreItem> storeList = AllItems.GetRange(0, AllItems.Count);

            if (currentSort != null) storeList = currentSort.Sort(storeList, currentSortDirection);

            storeLoader.LoadStoreWithPaging(storeList, 0);

        }
    }
}
