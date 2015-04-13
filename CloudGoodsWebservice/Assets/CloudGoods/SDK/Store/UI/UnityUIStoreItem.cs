using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;

namespace CloudGoods.SDK.Store.UI
{
    public class UnityUIStoreItem : MonoBehaviour
    {

        public StoreItem storeItem { get; private set; }

        public GameObject SalePanel;
        public Text SaleName;

        public GameObject StandardCurrencyFullWindow;
        public Text StandardCurrencyFullText;

        public GameObject PremiumCurrencyFullWindow;
        public Text PremiumCurrencyFullText;

        public GameObject CurrencyHalfWindow;
        public Text StandardCurrencyHalfText;
        public Text PremiumCurrencyHalfText;

        public Text StoreItemText;


        UnityUIStoreLoader storeLoader;

        void OnReceivedItemTexture(Texture2D texture)
        {
            if (gameObject == null) return;

            RawImage uiTexture = gameObject.GetComponentInChildren<RawImage>();
            uiTexture.texture = texture;
        }

        public virtual void Init(StoreItem item, UnityUIStoreLoader unityStoreLoader)
        {
            storeItem = item;
            storeLoader = unityStoreLoader;
            ItemTextureCache.GetItemTexture(storeItem.ItemInformation.ImageName, OnReceivedItemTexture);
            StoreItemText.text = storeItem.ItemInformation.Name;

            SetDisplayForSale();
            ChangePurchaseButtonDisplay(storeItem.CreditValue, storeItem.CoinValue);
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

        void SetDisplayForSale()
        {
            if (storeItem.Sale.Count > 0)
            {
                SalePanel.SetActive(true);
                SaleName.text = storeItem.Sale[0].SaleName;
            }
        }

        public void OnStoreItemClicked()
        {
            storeLoader.DisplayItemPurchasePanel(gameObject);
        }
    }
}
