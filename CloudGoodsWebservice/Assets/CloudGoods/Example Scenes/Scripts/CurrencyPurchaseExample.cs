using UnityEngine;
using System.Collections;
using CloudGoods.Services;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.Services.WebCommunication;

public class CurrencyPurchaseExample : MonoBehaviour {

    public GameObject PremiumCurrencyStore;

    void OnEnable()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
    }

    void OnDisable()
    {
        CallHandler.CloudGoodsInitilized -= CallHandler_CloudGoodsInitilized;
    }

	void Start () {
        CallHandler.Initialize();
	}

    void CallHandler_CloudGoodsInitilized()
    {
        AccountServices.Login(new LoginRequest("lionel.sy@gmail.com", "123456"), OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        PremiumCurrencyStore.SetActive(true);
    }
}
