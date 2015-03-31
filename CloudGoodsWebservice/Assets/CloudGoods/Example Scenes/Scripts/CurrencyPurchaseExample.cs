using UnityEngine;
using System.Collections;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Enums;

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
        CallHandler.Login(CloudGoodsPlatform.SocialPlay, "lionelsy", "lionel.sy@gmail.com", "123456", OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        PremiumCurrencyStore.SetActive(true);
    }
}
