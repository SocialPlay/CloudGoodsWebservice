using System;
using UnityEngine;
using LitJson;
using CloudGoods.SDK.Models;


namespace CloudGoods.CurrencyPurchase
{
    public class FaceBookPurchaser : MonoBehaviour, IPlatformPurchaser
    {
        public event Action<PurchasePremiumCurrencyBundleResponse> RecievedPurchaseResponse;
        public event Action<PurchasePremiumCurrencyBundleResponse> OnPurchaseErrorEvent;
        public int currentBundleID = 0;

        public IFacebookPurchase FacebookPurchasing;

        void Start()
        {
            FacebookPurchasing = this.gameObject.AddComponent(Type.GetType("FacebookPurchasing")) as IFacebookPurchase;
            FacebookPurchasing.Init();
        }

        public void Purchase(PremiumBundle bundleItem, int amount, string userID)
        {

            if (Type.GetType("FacebookPurchasing") != null)
            {
                FacebookPurchasing = this.gameObject.AddComponent(Type.GetType("FacebookPurchasing")) as IFacebookPurchase;
            }

            if (FacebookPurchasing == null)
            {
                Debug.LogError("Facebook purchase not found. Please add the FacebookPurchase script from the CloudGoodsFacebookAddon folder to this object and drag it as the public reference to the facebookPurchase variable in the inspector");
                return;
            }

            currentBundleID = int.Parse(bundleItem.BundleID);
            Console.WriteLine("Credit bundle purchase:  ID: " + bundleItem.BundleID + " Amount: " + amount);
            Debug.Log("ID: " + bundleItem.BundleID + "\nAmount: " + amount + "\nUserID: " + userID);
            FacebookPurchasing.Purchase(bundleItem, amount, OnReceivedFacebookCurrencyPurchase);
        }

        public void OnReceivedFacebookCurrencyPurchase(string data)
        {
            Debug.Log("data: " + data);
            JsonMapper.ToObject(data);

            //Newtonsoft.Json.Linq.JToken parsedData = Newtonsoft.Json.Linq.JToken.Parse(data);

            //if (parsedData["error_message"] != null)
            //{
            //    PurchasePremiumCurrencyBundleResponse response = new PurchasePremiumCurrencyBundleResponse();
            //    response.StatusCode = 0;
            //    response.Message = parsedData["error_message"].ToString();

            //    if (OnPurchaseErrorEvent != null) OnPurchaseErrorEvent(response);
            //    return;
            //}

            //Debug.Log("parsedData: " + parsedData.ToString());

            //BundlePurchaseRequest bundlePurchaseRequest = new BundlePurchaseRequest();
            //bundlePurchaseRequest.BundleID = currentBundleID;
            //bundlePurchaseRequest.UserID = CallHandler.User.userID.ToString();
            //bundlePurchaseRequest.ReceiptToken = parsedData["payment_id"].ToString();

            //TODO implement platform check for platform premium currency bundle purchase
            //bundlePurchaseRequest.PaymentPlatform = 1;

            //string bundleJsonString = JsonConvert.SerializeObject(bundlePurchaseRequest);

            //CloudGoods.PurchaseCreditBundles(bundleJsonString, OnReceivedPurchaseResponse);

            //if (RecievedPurchaseResponse != null)
            //    RecievedPurchaseResponse(data);

        }

        public void OnReceivedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
        {
            if (RecievedPurchaseResponse != null)
                RecievedPurchaseResponse(data);

            //CloudGoods.GetPremiumCurrencyBalance(null);
        }
    }
}

