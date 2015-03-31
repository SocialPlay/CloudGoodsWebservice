using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.Models;
using CloudGoods.Enums;
using CloudGoods.Utilities;

namespace CloudGoods.ItemBundles
{
    public class UnityUIItemBundle : MonoBehaviour
    {

        public UnityUIBundlePurchasing bundlePurchasing;
        public ItemBundleInfo itemBundle;

        public RawImage BundleImage;
        public Text Amount;

        public void SetupUnityUIItemBundle(ItemBundleInfo newItemBundle, UnityUIBundlePurchasing purchasing)
        {
            itemBundle = newItemBundle;
            bundlePurchasing = purchasing;

            ItemTextureCache.Instance.GetItemTexture(itemBundle.Image, OnReceivedItemTexture);

            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClickedItemBundle);
        }

        public void OnClickedItemBundle()
        {
            Debug.Log(itemBundle.Items.Count);
            bundlePurchasing.gameObject.SetActive(true);
            bundlePurchasing.SetupBundlePurchaseDetails(itemBundle);
        }

        void OnReceivedItemTexture(ImageStatus status, Texture2D texture)
        {
            BundleImage.texture = texture;
        }
    }
}