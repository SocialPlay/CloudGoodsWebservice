using UnityEngine;
using System.Collections;
using CloudGoods;
using CloudGoods.Models;
using CloudGoods.Enums;
using CloudGoods.Store;

public class StoreExample : MonoBehaviour {

    public GameObject store;

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
        store.SetActive(true);
        StoreInitializer initializer = store.GetComponent<StoreInitializer>();
        initializer.InitializeStore();
    }
}
