using UnityEngine;
using System.Collections;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Enums;
using CloudGoods.Store;
using CloudGoods.ItemBundles;

public class StoreExample : MonoBehaviour {

    public GameObject storeLoader;
    public UnityUIItemBundleLoader itemBundlesLoader;

    void Awake()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
        CallHandler.Initialize();
    }

    void CallHandler_CloudGoodsInitilized()
    {
        CallHandler.Login(CloudGoodsPlatform.SocialPlay, "lionelsy", "lionel.sy@gmail.com", "123456", OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        storeLoader.SetActive(true);
        StoreInitializer initializer = storeLoader.GetComponent<StoreInitializer>();
        itemBundlesLoader.GetItemBundles();
        initializer.InitializeStore();
    }
}
