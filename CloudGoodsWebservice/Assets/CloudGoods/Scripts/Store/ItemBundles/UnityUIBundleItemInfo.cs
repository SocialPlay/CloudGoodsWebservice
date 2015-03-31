using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Models;
using CloudGoods.Utilities;
using CloudGoods.Enums;

namespace CloudGoods.ItemBundles
{

    public class UnityUIBundleItemInfo : MonoBehaviour
    {

        public Text ItemName;
        public Text itemAmount;
        public Text ItemStats;

        public RawImage itemImage;

        public ItemBundleInfo.BundleItemInformation bundleItem;

        public void SetupBundleItemDisplay(ItemBundleInfo.BundleItemInformation newBundleItem)
        {
            bundleItem = newBundleItem;

            ItemName.text = bundleItem.Information.Name;
            itemAmount.text = "Amount: " + bundleItem.Amount;

            ItemStats.text = "";

            //foreach (BundleItemDetails details in newBundleItem.Information.Detail)
            //{
            //    ItemStats.text += details.BundleDetailName + " : " + details.Value + " \n";
            //}

            ItemTextureCache.Instance.GetItemTexture(bundleItem.Information.ImageName, OnReceivedItemTexture);
        }

        void OnReceivedItemTexture(ImageStatus imgStatus, Texture2D texture)
        {
            itemImage.texture = texture;
        }

    }
}
