using UnityEngine;
using System.Collections;
using System;
using LitJson;
using CloudGoods.SDK.Models;
using CloudGoods.Services;
using CloudGoods.Services.WebCommunication;

namespace CloudGoods.CurrencyPurchase
{
    public class iOSPremiumCurrencyPurchaser : MonoBehaviour, IPlatformPurchaser
    {
        public event Action<PurchasePremiumCurrencyBundleResponse> RecievedPurchaseResponse;
        public event Action<PurchasePremiumCurrencyBundleResponse> OnPurchaseErrorEvent;

        public int currentBundleID = 0;

        void Start()
        {
            iOSConnect.onReceivedMessage += OnReceivediOSMessageResponse;
            iOSConnect.onItemPurchaseCancelled += OnItemPurchaseCancelled;
            iOSConnect.onReceivedErrorOnPurchase += OnItemPurchaseError;
        }

        public void Purchase(PremiumBundle bundleItem, int amount, string userID)
        {
            Debug.Log("Purchase ios called");
            currentBundleID = int.Parse(bundleItem.BundleID);
            iOSConnect.RequestInAppPurchase(bundleItem.ProductID);
        }

        public void OnReceivediOSMessageResponse(string data)
        {
            SendReceiptTokenForVerification(data, 4);
        }

        void OnItemPurchaseCancelled(string cancelledString)
        {
            PurchasePremiumCurrencyBundleResponse response = new PurchasePremiumCurrencyBundleResponse();
            response.StatusCode = 0;
            response.Message = "User Cancelled";

            OnPurchaseErrorEvent(response);
        }

        void OnItemPurchaseError(string errorMessage)
        {
            PurchasePremiumCurrencyBundleResponse response = new PurchasePremiumCurrencyBundleResponse();
            response.StatusCode = 0;
            response.Message = "Error: " + errorMessage;

            OnPurchaseErrorEvent(response);
        }

        void SendReceiptTokenForVerification(string data, int platform)
        {
            BundlePurchaseRequest bundlePurchaseRequest = new BundlePurchaseRequest();
            bundlePurchaseRequest.BundleID = currentBundleID;
            bundlePurchaseRequest.ReceiptToken = data;
            bundlePurchaseRequest.PaymentPlatform = platform;

            CallHandler.Instance.PurchasePremiumCurrencyBundle(bundlePurchaseRequest, OnReceivedPurchaseResponse);
        }

        public void OnReceivedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
        {
            Debug.Log("received credit response: " + data);
            RecievedPurchaseResponse(data);
            CallHandler.Instance.GetPremiumCurrencyBalance(null);
        }
    }

    public class BundlePurchasePayload
    {
        public int BundleID;
        public int PaymentPlatform;
        public string ReceiptToken;
    }

}
