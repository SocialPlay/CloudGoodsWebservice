using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;

namespace CloudGoods.ItemBundles
{
    public class UnityUIItemBundle : MonoBehaviour
    {
        public UnityUIBundlePurchasing bundlePurchasing;
        public ItemBundleInfo itemBundle;

        public RawImage BundleImage;

        public GameObject StandardCurrencyFullWindow;
        public Text StandardCurrencyFullText;

        public GameObject PremiumCurrencyFullWindow;
        public Text PremiumCurrencyFullText;

        public GameObject CurrencyHalfWindow;
        public Text StandardCurrencyHalfText;
        public Text PremiumCurrencyHalfText;

        public Text StoreItemText;

        public void SetupUnityUIItemBundle(ItemBundleInfo newItemBundle, UnityUIBundlePurchasing purchasing)
        {
            itemBundle = newItemBundle;
            bundlePurchasing = purchasing;

            ItemTextureCache.GetItemTexture(itemBundle.Image, OnReceivedItemTexture);
            StoreItemText.text = itemBundle.Name;

            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClickedItemBundle);

            ChangePurchaseButtonDisplay(itemBundle.PremiumPrice, itemBundle.StandardPrice);
        }

        private void ChangePurchaseButtonDisplay(int itemCreditCost, int itemCoinCost)
        {
            StandardCurrencyFullWindow.SetActive(false);
            PremiumCurrencyFullWindow.SetActive(false);
            CurrencyHalfWindow.SetActive(false);

            if (itemCreditCost > 0 && itemCoinCost > 0)
            {
                CurrencyHalfWindow.SetActive(true);
                StandardCurrencyHalfText.text = itemCoinCost.ToString();
                PremiumCurrencyHalfText.text = itemCreditCost.ToString();
            }
            else if (itemCreditCost < 0)
            {
                StandardCurrencyFullWindow.SetActive(true);
                StandardCurrencyFullText.text = itemCoinCost.ToString();
            }
            else if (itemCoinCost < 0)
            {
                PremiumCurrencyFullWindow.SetActive(true);
                PremiumCurrencyFullText.text = itemCreditCost.ToString();
            }
            else
            {
                CurrencyHalfWindow.SetActive(true);
                StandardCurrencyHalfText.text = itemCoinCost.ToString();
                PremiumCurrencyHalfText.text = itemCreditCost.ToString();
            }
        }

        public void OnClickedItemBundle()
        {
            bundlePurchasing.gameObject.SetActive(true);
            bundlePurchasing.SetupBundlePurchaseDetails(itemBundle);
        }

        void OnReceivedItemTexture( Texture2D texture)
        {
            BundleImage.texture = texture;
        }
    }
}