using UnityEngine;
using System.Collections;
using CloudGoods.Services;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoods.SDK.Store;
using CloudGoods.ItemBundles;
using CloudGoods.Services.WebCommunication;

public class StoreExample : MonoBehaviour
{

    public GameObject storeLoader;
    public UnityUIItemBundleLoader itemBundlesLoader;

    void Awake()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
        CallHandler.Initialize();
    }

    void CallHandler_CloudGoodsInitilized()
    {
        AccountServices.Login(new LoginRequest("lionel.sy@gmail.com", "123456"), OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        storeLoader.SetActive(true);
        StoreInitializer initializer = storeLoader.GetComponent<StoreInitializer>();
        initializer.InitializeStore();
    }
}
