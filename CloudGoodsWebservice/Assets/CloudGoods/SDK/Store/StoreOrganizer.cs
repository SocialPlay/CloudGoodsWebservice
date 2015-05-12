using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Store;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;
using CloudGoods.SDK.Models;
using System;

namespace CloudGoods.SDK.Store
{
    public class StoreOrganizer : MonoBehaviour
    {
        public StoreLoader storeLoader;
        public InputValueChange SearchInput;
        private ISortItem currentSort;
        private int currentSortDirection = 1;
        public string searchFilter = "";


        void OnEnable()
        {
            SearchInput.searchUpdate += SearchNameFilter_searchUpdate;
        }

        void OnDisable()
        {
            SearchInput.searchUpdate -= SearchNameFilter_searchUpdate;
        }

        void SortStoreItemsBy_SortUpdate(ISortItem CurrentSort, int direction)
        {
            currentSort = CurrentSort;
            currentSortDirection = direction;
            OrganizeStore();
        }

        void SearchNameFilter_searchUpdate(string searchFilter)
        {
            Debug.Log("searchFilter");
            this.searchFilter = searchFilter;
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

            storeList = SearchStoreItems(storeList, searchFilter);

            if (currentSort != null) storeList = currentSort.Sort(storeList, currentSortDirection);

            storeLoader.LoadStoreWithPaging(storeList, 0);

        }

        public List<StoreItem> SearchStoreItems(List<StoreItem> storeItems, string searchFilter)
        {
            if (searchFilter.Length == 0)
            {
                return storeItems;
            }

            List<StoreItem> filteredStoreItems = new List<StoreItem>();
            for (int i = 0, imax = storeItems.Count; i < imax; i++)
            {
                StoreItem storeItemInfo = storeItems[i];
                if (storeItemInfo.ItemInformation.Name.StartsWith(searchFilter.ToLower(), StringComparison.InvariantCultureIgnoreCase)) filteredStoreItems.Add(storeItemInfo);
            }
            return filteredStoreItems;
        }

    }
}
