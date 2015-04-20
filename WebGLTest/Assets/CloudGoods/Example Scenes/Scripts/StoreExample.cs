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
        Debug.LogError("Start");
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
        CallHandler.Initialize();
    }

    void CallHandler_CloudGoodsInitilized()
    {

        Debug.LogError("Initialized");
        AccountServices.Login("lionel.sy@gmail.com", "123456", OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        storeLoader.SetActive(true);
        StoreInitializer initializer = storeLoader.GetComponent<StoreInitializer>();
        initializer.InitializeStore();
    }
}
