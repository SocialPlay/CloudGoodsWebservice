using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Store.UI;
using CloudGoods.Services;


namespace CloudGoods.ItemBundles
{
    public class UnityUIBundlePurchasing : MonoBehaviour
    {

        public static Action<ItemBundlePurchaseResponse> OnPurchaseSuccessful;

        public GameObject PremiumCurrencyPurchaseWindowHalf;
        public GameObject PremiumCurrencyPurchaseWindowFull;
        public GameObject StandardCurrencyPurchaseWindowHalf;
        public GameObject StandardCurrencyPurchaseWindowFull;

        public GameObject bundleItemDisplayPrefab;

        public Text BundleName;

        public int purchaseContainerLocation = 0;

        public ItemBundleInfo currentItemBundle;

        List<GameObject> bundleObjects = new List<GameObject>();

        public GameObject bundleGrid;


        public void ClosePurchaseWindow()
        {
            gameObject.SetActive(false);
        }

        public void SetupBundlePurchaseDetails(ItemBundleInfo bundle)
        {
            CloudGoodsBundle bundlePriceState;

            if (bundle.StandardPrice <= 0 && bundle.PremiumPrice <= 0)
                bundlePriceState = CloudGoodsBundle.Free;
            else if (bundle.StandardPrice <= 0)
                bundlePriceState = CloudGoodsBundle.CreditPurchasable;
            else if (bundle.PremiumPrice <= 0)
                bundlePriceState = CloudGoodsBundle.CoinPurchasable;
            else
                bundlePriceState = CloudGoodsBundle.CreditCoinPurchaseable;

            ChangePurchaseButtonDisplay(bundle.PremiumPrice, bundle.StandardPrice, bundlePriceState);

            currentItemBundle = bundle;

            BundleName.text = bundle.Name;
            SetUpBundleItemsDisplay(bundle.Items);
        }

        void SetUpBundleItemsDisplay(List<ItemBundleInfo.BundleItemInformation> bundleItems)
        {
            ClearGrid(bundleGrid);

            foreach (ItemBundleInfo.BundleItemInformation bundleItem in bundleItems)
            {
                GameObject bundleItemObj = (GameObject)GameObject.Instantiate(bundleItemDisplayPrefab);

                bundleItemObj.transform.SetParent(bundleGrid.transform);

                UnityUIBundleItemInfo bundleInfo = bundleItemObj.GetComponent<UnityUIBundleItemInfo>();
                bundleInfo.SetupBundleItemDisplay(bundleItem);

                bundleObjects.Add(bundleItemObj);
            }
        }

        private void ChangePurchaseButtonDisplay(int itemCreditCost, int itemCoinCost, CloudGoodsBundle state)
        {
            Debug.Log("State: " + state.ToString());
            switch (state)
            {
                case CloudGoodsBundle.CreditPurchasable:
                    StandardCurrencyPurchaseWindowFull.SetActive(false);
                    StandardCurrencyPurchaseWindowHalf.SetActive(false);
                    PremiumCurrencyPurchaseWindowFull.SetActive(true);
                    PremiumCurrencyPurchaseWindowHalf.SetActive(false);

                    UnityUIPurchaseButtonDisplay premiumButtonDisplay = PremiumCurrencyPurchaseWindowFull.GetComponent<UnityUIPurchaseButtonDisplay>();
                    premiumButtonDisplay.SetState(itemCreditCost);
                    break;
                case CloudGoodsBundle.CoinPurchasable:
                    StandardCurrencyPurchaseWindowFull.SetActive(true);
                    StandardCurrencyPurchaseWindowHalf.SetActive(false);
                    PremiumCurrencyPurchaseWindowFull.SetActive(false);
                    PremiumCurrencyPurchaseWindowHalf.SetActive(false);

                    UnityUIPurchaseButtonDisplay standardButtonDisplay = StandardCurrencyPurchaseWindowFull.GetComponent<UnityUIPurchaseButtonDisplay>();
                    standardButtonDisplay.SetState(itemCoinCost);
                    break;
                case CloudGoodsBundle.Free:
                    StandardCurrencyPurchaseWindowFull.SetActive(false);
                    StandardCurrencyPurchaseWindowHalf.SetActive(false);
                    PremiumCurrencyPurchaseWindowFull.SetActive(true);
                    PremiumCurrencyPurchaseWindowHalf.SetActive(false);

                    UnityUIPurchaseButtonDisplay standardButtonDisplayFree = StandardCurrencyPurchaseWindowFull.GetComponent<UnityUIPurchaseButtonDisplay>();
                    standardButtonDisplayFree.SetState(itemCoinCost);
                    break;
                default:
                    StandardCurrencyPurchaseWindowFull.SetActive(false);
                    StandardCurrencyPurchaseWindowHalf.SetActive(true);
                    PremiumCurrencyPurchaseWindowFull.SetActive(false);
                    PremiumCurrencyPurchaseWindowHalf.SetActive(true);

                    UnityUIPurchaseButtonDisplay standardButtonDisplayDefault = StandardCurrencyPurchaseWindowHalf.GetComponent<UnityUIPurchaseButtonDisplay>();
                    standardButtonDisplayDefault.SetState(itemCoinCost);

                    UnityUIPurchaseButtonDisplay PremiumButtonDisplayDefault = PremiumCurrencyPurchaseWindowHalf.GetComponent<UnityUIPurchaseButtonDisplay>();
                    PremiumButtonDisplayDefault.SetState(itemCreditCost);
                    break;
            }
        }

        private void ClearGrid(GameObject gridObj)
        {
            gridObj.transform.DetachChildren();

            foreach (GameObject bundleObject in bundleObjects)
            {
                Destroy(bundleObject);
            }

            bundleObjects.Clear();
        }

        public void PurchaseBundleWithStandardCurrency()
        {
            ItemStoreServices.PurchaseItemBundle(new ItemBundlePurchaseRequest(currentItemBundle.Id, (int)CurrencyType.Standard, purchaseContainerLocation), OnReceivedPurchaseCallback);
            ClosePurchaseWindow();
        }

        public void PurchaseBundleWithPremiumCurrency()
        {
            ItemStoreServices.PurchaseItemBundle(new ItemBundlePurchaseRequest(currentItemBundle.Id, (int)CurrencyType.Premium, purchaseContainerLocation), OnReceivedPurchaseCallback);
            ClosePurchaseWindow();
        }

        void OnReceivedPurchaseCallback(ItemBundlePurchaseResponse response)
        {
            Debug.Log("Successfully purchased item bundle:" + response.PremiumBalance);

            ItemStoreServices.GetPremiumCurrencyBalance(null);
            ItemStoreServices.GetStandardCurrencyBalance(new StandardCurrencyBalanceRequest(0), null);

            if (OnPurchaseSuccessful != null)
                OnPurchaseSuccessful(response);
        }
    }
}
