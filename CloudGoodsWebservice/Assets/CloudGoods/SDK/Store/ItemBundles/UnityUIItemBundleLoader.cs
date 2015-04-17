using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Services;


namespace CloudGoods.ItemBundles
{
    public class UnityUIItemBundleLoader : MonoBehaviour
    {

        public UnityUIBundlePurchasing bundlePurchasing;
        public GameObject ItemBundleButtonObject;

        public GameObject gridObject;

        List<GameObject> ItemBundleObj = new List<GameObject>();

        public void GetItemBundles()
        {
            ItemStoreServices.GetItemBundles(new ItemBundlesRequest(), CloudGoods_OnStoreItemBundleListLoaded);
        }

        void CloudGoods_OnStoreItemBundleListLoaded(ItemBundlesResponse obj)
        {
            LoadBundleItems(obj.bundles);
        }

        public void LoadBundleItems(List<ItemBundleInfo> itemBundles)
        {

            ClearCurrentGrid();

            foreach (ItemBundleInfo bundle in itemBundles)
            {
                GameObject newItemBundle = (GameObject)GameObject.Instantiate(ItemBundleButtonObject);
                UnityUIItemBundle ItemBundle = newItemBundle.GetComponent<UnityUIItemBundle>();

                newItemBundle.transform.SetParent(gridObject.transform);
                newItemBundle.transform.localScale = new Vector3(1, 1, 1);

                ItemBundle.SetupUnityUIItemBundle(bundle, bundlePurchasing);

                ItemBundleObj.Add(newItemBundle);
            }
        }

        void ClearCurrentGrid()
        {
            foreach (GameObject gridItemObj in ItemBundleObj)
            {
                Destroy(gridItemObj);
            }
            ItemBundleObj.Clear();
        }
    }
}