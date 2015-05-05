// ----------------------------------------------------------------------
// <copyright file="CreditBundleStore.cs" company="SocialPlay">
//     Copyright statement. All right reserved
// </copyright>
// Owner: Alex Zanfir
// Date: 11/2/2012
// Description: This is a store that sells only credit bundles, to allow from native currency to our currency, if we choose not to support direct buy with platforms native currency.
// ------------------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Utilities;
using CloudGoods.Services;

namespace CloudGoods.CurrencyPurchase
{
    public class PremiumCurrencyBundleStore : MonoBehaviour
    {
        public static event Action<PurchasePremiumCurrencyBundleResponse> OnPremiumCurrencyPurchased;
        public GameObject Grid;
        [HideInInspector]
        public bool isInitialized = false;
        private bool isWaitingForPlatform = false;

        IGridLoader gridLoader;
        public IPlatformPurchaser platformPurchasor;
        bool isPurchaseRequest = false;

        string domain;

        public CurrencyType type = CurrencyType.Standard;

        void Start()
        {
            this.gameObject.name = "PremiumCurrencyBundleStore";
            Initialize();
        }


        void OnRegisteredUserToSession(string obj)
        {
            if (!isInitialized) Initialize();
            Debug.Log("OnUser Registered");
        }

        public void Initialize()
        {
            Debug.Log("Initialize Credit Bundles");
            ItemStoreServices.GetPremiumCurrencyBalance(null);

            switch (BuildPlatform.Platform)
            {
                case BuildPlatform.BuildPlatformType.Automatic:
                    if (isWaitingForPlatform) return;
                    isWaitingForPlatform = true;
                    BuildPlatform.OnBuildPlatformFound += (platform) =>
                    {
                        Debug.Log("Recived new build platform");
                        Initialize();
                    };
                    return;
                case BuildPlatform.BuildPlatformType.Facebook:
                    platformPurchasor = gameObject.AddComponent<FaceBookPurchaser>();
                    break;
                case BuildPlatform.BuildPlatformType.Kongergate:
                    platformPurchasor = gameObject.AddComponent<KongregatePurchase>();
                    break;
                case BuildPlatform.BuildPlatformType.Android:
                    platformPurchasor = gameObject.AddComponent<AndroidPremiumCurrencyPurchaser>();
                    break;
                case BuildPlatform.BuildPlatformType.IOS:
                    platformPurchasor = gameObject.AddComponent<iOSPremiumCurrencyPurchaser>();
                    GameObject o = new GameObject("iOSConnect");
                    //o.AddComponent<iOSConnect>();
                    break;
                case BuildPlatform.BuildPlatformType.CloudGoodsStandAlone:
                    Debug.LogWarning("Cloud Goods Stand alone has not purchase method currently.");
                    break;
            }

            if (platformPurchasor == null)
            {
                Debug.Log("platform purchasor is null");
                return;
            }

            platformPurchasor.RecievedPurchaseResponse += OnRecievedPurchaseResponse;
            platformPurchasor.OnPurchaseErrorEvent += platformPurchasor_OnPurchaseErrorEvent;

            if (BuildPlatform.Platform == BuildPlatform.BuildPlatformType.EditorTestPurchasing)
            {
                Debug.Log("Get credit bundles from editor");

                ItemStoreServices.GetPremiumBundles(new PremiumBundlesRequest(1), OnPurchaseBundlesRecieved);
            }
            else
            {
                Debug.Log("Purchasing credit bundles from platform:" + BuildPlatform.Platform);
                ItemStoreServices.GetPremiumBundles(new PremiumBundlesRequest((int)BuildPlatform.Platform), OnPurchaseBundlesRecieved);
            }

            isInitialized = true;
        }


        void OnDisable()
        {
            if (platformPurchasor != null) platformPurchasor.RecievedPurchaseResponse -= OnRecievedPurchaseResponse;
        }

        void OnPurchaseBundlesRecieved(List<PremiumCurrencyBundle> data)
        {
            Debug.Log("purchase bundles: " + data);

            Debug.Log("Got credit bundles");
            gridLoader = (IGridLoader)Grid.GetComponent(typeof(IGridLoader));
            gridLoader.ItemAdded += OnItemInGrid;
            gridLoader.LoadGrid(data);
        }

        void OnItemInGrid(PremiumCurrencyBundle item, GameObject obj)
        {
            PremiumBundle creditBundle = obj.GetComponent<PremiumBundle>();
            creditBundle.Amount = item.CreditAmount.ToString();
            creditBundle.Cost = item.Cost.ToString();

            if (item.Data.Count > 0)
            {
                creditBundle.ProductID = item.Data[0].Value;
            }

            //if (item.CreditPlatformIDs.ContainsKey("IOS_Product_ID"))
            //    creditBundle.ProductID = item.CreditPlatformIDs["IOS_Product_ID"].ToString();

            creditBundle.BundleID = item.ID.ToString();

            creditBundle.PremiumCurrencyName = "";
            creditBundle.Description = item.Description;


            if (!string.IsNullOrEmpty(item.Image))
            {
                ItemTextureCache.GetItemTexture(item.Image, delegate(Texture2D texture)
                {
                    creditBundle.SetIcon(texture);
                });
            }

            creditBundle.SetBundleName(item.Name);

            creditBundle.OnPurchaseRequest = OnPurchaseRequest;
        }

        void OnPurchaseRequest(PremiumBundle item)
        {
            if (!isPurchaseRequest)
            {
                isPurchaseRequest = true;
                platformPurchasor.Purchase(item, 1, AccountServices.ActiveUser.UserID.ToString());
            }
        }

        void OnRecievedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
        {
            Debug.Log("Received purchase response:  " + data);
            isPurchaseRequest = false;

            if (OnPremiumCurrencyPurchased != null)
                OnPremiumCurrencyPurchased(data);
        }

        void platformPurchasor_OnPurchaseErrorEvent(PurchasePremiumCurrencyBundleResponse obj)
        {
            Debug.LogError("Purchase Platform Error: " + obj);

            isPurchaseRequest = false;
        }

    }
}