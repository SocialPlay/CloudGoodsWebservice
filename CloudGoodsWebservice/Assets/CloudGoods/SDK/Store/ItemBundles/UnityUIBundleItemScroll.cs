using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;

namespace CloudGoods.ItemBundles
{
    public class UnityUIBundleItemScroll : MonoBehaviour
    {

        public Text ItemName;
        public Text ItemDetails;
        public RawImage ItemImage;

        public UnityUIBundlePurchasing bundlePurchasing;

        ItemBundleInfo itemBundle;

        int currentBundleIndex = 0;

        void Start()
        {
            currentBundleIndex = 0;
            itemBundle = bundlePurchasing.currentItemBundle;
            SetBundleItemToDisplay(currentBundleIndex);
        }

        void Update()
        {
            if (bundlePurchasing.currentItemBundle != itemBundle)
            {
                currentBundleIndex = 0;
                itemBundle = bundlePurchasing.currentItemBundle;
                SetBundleItemToDisplay(currentBundleIndex);
            }
        }

        void SetBundleItemToDisplay(int index)
        {
            ItemBundleInfo.BundleItemInformation bundleitem = itemBundle.Items[index];

            ItemName.text = bundleitem.Information.Name;

            string formated = "";

            //foreach (BundleItemDetails detail in bundleitem.bundleItemDetails)
            //{
            //    formated = string.Format("{0}{1}: {2}\n", formated, detail.BundleDetailName, detail.Value.ToString());
            //}

            ItemDetails.text = formated;

            ItemTextureCache.GetItemTexture(bundleitem.Information.ImageName, OnReceivedItemTexture);
        }

        void OnReceivedItemTexture( Texture2D newTexture)
        {
            ItemImage.texture = newTexture;
        }

        public void DisplayNextBundleItem()
        {
            if (currentBundleIndex >= itemBundle.Items.Count - 1)
                return;

            else
            {
                currentBundleIndex++;
                SetBundleItemToDisplay(currentBundleIndex);
            }
        }

        public void DisplayPreviousBundleItem()
        {
            if (currentBundleIndex <= 0)
                return;

            else
            {
                currentBundleIndex--;
                SetBundleItemToDisplay(currentBundleIndex);
            }
        }
    }
}