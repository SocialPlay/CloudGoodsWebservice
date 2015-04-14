using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.Models;
using CloudGoods.Enums;
using CloudGoods.Utilities;

namespace CloudGoods.Store.UI
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

        void OnReceivedItemTexture(ImageStatus imageStatus, Texture2D texture)
        {
            if (gameObject == null) return;

            RawImage uiTexture = gameObject.GetComponentInChildren<RawImage>();
            uiTexture.texture = texture;
        }

        public virtual void Init(StoreItem item, UnityUIStoreLoader unityStoreLoader)
        {
            storeItem = item;
            storeLoader = unityStoreLoader;
            ItemTextureCache.Instance.GetItemTexture(storeItem.ItemInformation.ImageName, OnReceivedItemTexture);
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
                tmpPremiumCost = storeItem.Sale[0].PremiumCurrencySaleValue;
                tmpStandardCost = storeItem.Sale[0].StandardCurrencySaleValue;
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
                SaleName.text = storeItem.Sale[0].SaleName;
                IsSale = true;

                ChangeSalePriceDisplay();
            }

            ChangePurchaseButtonDisplay();
        }

        void ChangeSalePriceDisplay()
        {
            StandardCurrencyFullWindow.GetComponent<Image>().color = Color.green;
            PremiumCurrencyFullWindow.GetComponent<Image>().color = Color.green;
            Image[] tmpImages = CurrencyHalfWindow.GetComponentsInChildren<Image>();

            tmpImages[2].color = Color.green;
            tmpImages[1].color = Color.green;
            
        }

        public void OnStoreItemClicked()
        {
            storeLoader.DisplayItemPurchasePanel(gameObject);
        }
    }
}
