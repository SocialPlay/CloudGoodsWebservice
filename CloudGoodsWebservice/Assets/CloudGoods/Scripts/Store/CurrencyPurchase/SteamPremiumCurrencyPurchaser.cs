using UnityEngine;
using System.Collections;
using CloudGoods.CurrencyPurchase;
using System;
using CloudGoods.SDK.Models;
using System.Collections.Generic;
using CloudGoods;
using System.Text;



public class SteamPremiumCurrencyPurchaser : MonoBehaviour, IPlatformPurchaser {

    public event Action<PurchasePremiumCurrencyBundleResponse> RecievedPurchaseResponse;

    public event Action<PurchasePremiumCurrencyBundleResponse> OnPurchaseErrorEvent;

    public void Purchase(PremiumBundle id, int amount, string userID)
    {

        var url = "http://api.steampowered.com/ISteamMicroTxnSandbox/InitTxn/v0002/";

        string objectString = LitJson.JsonMapper.ToJson(new SteamPurchaseRequest());
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        byte[] body = Encoding.UTF8.GetBytes(objectString);
        PurchaseSteamBundle(new WWW(url, body, headers), OnReceivedPurchaseResponse);
    }

    public class SteamPurchaseRequest
    {
        public string orderid = "1234566546546461518";
        public string steamid = "76561197974087399";
        public string appid = "335710";
        public int itemcount = 1;
        public string language = "EN";
        public string currency = "CAD";
        public string[] itemid = new string[1]{"1122315"};
        public int[] qty = new int[1]{1};
        public int[] amount = new int[1]{199};
        public string[] description = new string[1]{"testBundle"};
        public string key = "A39B8A4076704311E6C121089A022B94";
    }
    

    IEnumerator PurchaseSteamBundle(WWW www, Action<PurchasePremiumCurrencyBundleResponse> callback)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            //callback(www.text);
            Debug.Log("Returned call: " + www.text);
        }
        else
        {
            Debug.Log(www.text);
            Debug.LogError("Error: " + www.error);
            Debug.LogError("Error: " + www.url);
        }
    }

    public void OnReceivedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
    {
        throw new System.NotImplementedException();
    }
}
