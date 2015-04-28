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

        bool IsSale = false;

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

            SetPriceDisplay();
        }

        private void ChangePurchaseButtonDisplay()
        {
            StandardCurrencyFullWindow.SetActive(false);
            PremiumCurrencyFullWindow.SetActive(false);
            CurrencyHalfWindow.SetActive(false);

            int tmpPremiumCost;
            int tmpStandardCost;

            if (IsSale)
            {
                if(storeItem.Sale[0].PremiumCurrencySaleValue > 0)
                    tmpPremiumCost = storeItem.Sale[0].PremiumCurrencySaleValue;
                else
                    tmpPremiumCost = storeItem.CreditValue;

                if(storeItem.Sale[0].StandardCurrencySaleValue > 0)
                    tmpStandardCost = storeItem.Sale[0].StandardCurrencySaleValue;
                else
                    tmpStandardCost = storeItem.CoinValue;
            }
            else
            {
                tmpPremiumCost = storeItem.CreditValue;
                tmpStandardCost = storeItem.CoinValue;
            }

            if (tmpPremiumCost > 0 && tmpStandardCost > 0)
            {
                CurrencyHalfWindow.SetActive(true);

                StandardCurrencyHalfText.text = tmpStandardCost.ToString();
                PremiumCurrencyHalfText.text = tmpPremiumCost.ToString();
            }
            else if(tmpPremiumCost < 0 && tmpStandardCost < 0)
            {
                StandardCurrencyFullWindow.SetActive(true);
                StandardCurrencyFullText.text = "0";
            }
            else if (tmpPremiumCost < 0)
            {
                StandardCurrencyFullWindow.SetActive(true);
                StandardCurrencyFullText.text = tmpStandardCost.ToString();
            }
            else if (tmpStandardCost < 0)
            {
                PremiumCurrencyFullWindow.SetActive(true);
                PremiumCurrencyFullText.text = tmpPremiumCost.ToString();
            }
            else
            {
                CurrencyHalfWindow.SetActive(true);
                StandardCurrencyHalfText.text = tmpStandardCost.ToString();
                PremiumCurrencyHalfText.text = tmpPremiumCost.ToString();
            }
        }

        void SetPriceDisplay()
        {
            if (storeItem.Sale.Count > 0)
            {
                SalePanel.SetActive(true);
                SaleName.text = storeItem.ItemInformation.Name;
                IsSale = true;

                ChangeSalePriceDisplay(storeItem.Sale[0]);
            }

            ChangePurchaseButtonDisplay();
        }

        void ChangeSalePriceDisplay(SalePrices salePrices)
        {

            Image[] tmpImages = CurrencyHalfWindow.GetComponentsInChildren<Image>();

            if (salePrices.StandardCurrencySaleValue > 0)
            {
                StandardCurrencyFullWindow.GetComponent<Image>().color = Color.green;
                tmpImages[1].color = Color.green;
            }
            else
            {
                StandardCurrencyFullWindow.GetComponent<Image>().color = Color.white;
                tmpImages[1].color = Color.white;
            }

            if (salePrices.PremiumCurrencySaleValue > 0)
            {
                PremiumCurrencyFullWindow.GetComponent<Image>().color = Color.green;
                tmpImages[2].color = Color.green;
            }
            else
            {
                StandardCurrencyFullWindow.GetComponent<Image>().color = Color.white;
                tmpImages[2].color = Color.white;
            }
            
        }

        public void OnStoreItemClicked()
        {
            storeLoader.DisplayItemPurchasePanel(gameObject);
        }
    }
}
