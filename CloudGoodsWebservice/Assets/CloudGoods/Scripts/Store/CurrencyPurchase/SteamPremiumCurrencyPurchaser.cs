using UnityEngine;
using System.Collections;
using CloudGoods.CurrencyPurchase;
using System;
using CloudGoods.SDK.Models;
using System.Collections.Generic;
using CloudGoods;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using CloudGoods.Services.Webservice;
using Steamworks;
using CloudGoods.Services.WebCommunication;
using LitJson;

public class SteamPremiumCurrencyPurchaser : MonoBehaviour, IPlatformPurchaser {

    public event Action<PurchasePremiumCurrencyBundleResponse> RecievedPurchaseResponse;

    public event Action<PurchasePremiumCurrencyBundleResponse> OnPurchaseErrorEvent;

    CallObjectCreator callCreator = new WebAPICallObjectCreator();
    ResponseCreator responseCreator = new LitJsonResponseCreator();

    PremiumBundle currentPremiumBundle;

    protected Callback<MicroTxnAuthorizationResponse_t> microTransactionResponse;

    void OnEnable()
    {
        microTransactionResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTransactionResponse);
    }


    public void Purchase(PremiumBundle bundle, int amount, string userID)
    {
        currentPremiumBundle = bundle;

        SteamPurchaseRequest request = new SteamPurchaseRequest();
        request.bundleId = int.Parse(bundle.BundleID);
        request.SteamUserId = SteamUser.GetSteamID().ToString();

        StartCoroutine(ServiceGetString(callCreator.CreateSteamPremiumPurchaseCall(request), x =>
        {
            //TODO Handle error for error when trying to pop up transaction window from steam
            SteamCurrencyTransactionResponse txnResponse = JsonMapper.ToObject<SteamCurrencyTransactionResponse>(JsonMapper.ToObject(x)["response"].ToJson());

            if (txnResponse.result == "OK")
            {
                //Transaction Window pop up successfull
            }
            else
            {
                //Error occured when trying to pop up steam transaction window
            }
        }));
    }

    void OnMicroTransactionResponse(MicroTxnAuthorizationResponse_t response)
    {
        if (response.m_bAuthorized == 0)
            OnPurchaseErrorEvent(new PurchasePremiumCurrencyBundleResponse()
            {
                StatusCode = 0,
                Balance = 0,
                Message = "Transaction Cancelled"
            });
        else
            SubmitSteamOrder(response);
    }

    void SubmitSteamOrder(MicroTxnAuthorizationResponse_t response)
    {
        SteamOrderConfirmationRequest request = new SteamOrderConfirmationRequest()
        {
            AppId = response.m_unAppID.ToString(),
            OrderId = int.Parse(response.m_ulOrderID.ToString()),
            BundleId = int.Parse(currentPremiumBundle.BundleID)

        };

        StartCoroutine(ServiceGetString(callCreator.CreateSteamOrderConfirmationCall(request), x =>
        {
            Debug.LogError("Received Confirmation: " + x);

            SteamCurrencyTransactionResponse txnResponse = JsonMapper.ToObject<SteamCurrencyTransactionResponse>(JsonMapper.ToObject(x)["response"].ToJson());

            if (txnResponse.result == "OK")
            {
                OnReceivedPurchaseResponse(new PurchasePremiumCurrencyBundleResponse()
                    {
                        Message = "Transaction Complete"
                    });
                CallHandler.Instance.GetPremiumCurrencyBalance(null);
            }
            else
            {
                OnPurchaseErrorEvent(new PurchasePremiumCurrencyBundleResponse()
                {
                    Message = "Transaction Error"
                });
            }
        }));
    }

    public void OnReceivedPurchaseResponse(PurchasePremiumCurrencyBundleResponse data)
    {
        RecievedPurchaseResponse(data);
    }

    IEnumerator ServiceGetString(WWW www, Action<string> callback)
    {
        yield return www;

        if (www.error == null)
        {
                callback(www.text);
        }
        else
        {
            Debug.LogError("Error: " + www.error);
            Debug.LogError("Error: " + www.url);
        }
    }
}
