using UnityEngine;
using System.Collections;
using CloudGoods.Models;
using CloudGoods;
using CloudGoods.Enums;
using System.Collections.Generic;

public class TestWebservice : MonoBehaviour {

    void OnEnable()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
    }

    void OnDisable()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
    }

	// Use this for initialization
	void Start () {
        CallHandler.Initialize();
	}

    void CallHandler_CloudGoodsInitilized()
    {
        CallHandler.Login(CloudGoodsPlatform.SocialPlay, "lionel", "lionel.sy@gmail.com", "123456", OnReceivedLoginResponse);
    }

    void OnReceivedLoginResponse(CloudGoodsUser user)
    {
        CallHandler.GetCurrencyInfo(OnReceivedCurrencyInfo);
        CallHandler.GetPremiumCurrencyBalance(OnReceivedPremiumCurrencyBalance);
        CallHandler.GetStandardCurrencyBalance(0, OnReceivedStandardCurrencyBalance);
        CallHandler.GetStoreItems(OnReceivedStoreItems);
    }

    void OnReceivedStoreItems(List<StoreItem> storeItems)
    {
        Debug.Log("store items count: " + storeItems.Count);

        foreach(StoreItem storeItem in storeItems)
        {
            Debug.Log("store item found: " + storeItem.Name);

            foreach(StoreItemDetail detail in storeItem.ItemDetails)
            {
                Debug.Log(detail.Name);
            }
        }
    }

    void OnReceivedPremiumCurrencyBalance(CurrencyBalanceResponse balance)
    {
        Debug.Log("Premium Currency Balance: " + balance.Amount);
    }

    void OnReceivedStandardCurrencyBalance(SimpleItemInfo balance)
    {
        Debug.Log("Standard Currency Balance: " + balance.Amount);
        Debug.Log("Standard Currency Location: " + balance.Location);
        Debug.Log("Standard Currency StackID: " + balance.StackId);
    }

	void OnReceivedCurrencyInfo(CurrencyInfoResponse response)
    {
        Debug.Log("Premium name : " + response.PremiumCurrencyName);
        Debug.Log("Premium Image : " + response.PremiumCurrencyImage);
        Debug.Log("Standard name : " + response.StandardCurrencyName);
        Debug.Log("Standard Image : " + response.StandardCurrencyImage);
    }
}
