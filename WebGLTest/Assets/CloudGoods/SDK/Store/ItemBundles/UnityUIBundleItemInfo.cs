using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Utilities;
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

            ItemTextureCache.GetItemTexture(bundleItem.Information.ImageName, OnReceivedItemTexture);
        }

        void OnReceivedItemTexture( Texture2D texture)
        {
            itemImage.texture = texture;
        }

    }
}
