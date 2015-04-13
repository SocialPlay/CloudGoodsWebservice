using UnityEngine;
using System.Collections;
using CloudGoods.ItemBundles;
using CloudGoods.Models;
using CloudGoods;

public class ItemBundleStoreExample : MonoBehaviour {

    public UnityUIItemBundleLoader itemBundlesLoader;
    public GameObject StoreDisplay;

    void Awake()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
        CallHandler.Initialize();
    }

    void CallHandler_CloudGoodsInitilized()
    {
        CallHandler.Login("lionel.sy@gmail.com", "123456", OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        StoreDisplay.SetActive(true);
        UpdateUserCurrency();
        itemBundlesLoader.GetItemBundles();
    }

    public void UpdateUserCurrency()
    {
        CallHandler.GetStandardCurrencyBalance(0, null);
        CallHandler.GetPremiumCurrencyBalance(null);
    }
}
