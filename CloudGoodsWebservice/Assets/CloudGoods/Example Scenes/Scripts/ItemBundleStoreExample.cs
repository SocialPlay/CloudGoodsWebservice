using UnityEngine;
using System.Collections;
using CloudGoods.ItemBundles;
using CloudGoods;
using CloudGoods.SDK.Models;
using CloudGoods.Services.WebCommunication;

public class ItemBundleStoreExample : MonoBehaviour
{

    public UnityUIItemBundleLoader itemBundlesLoader;
    public GameObject StoreDisplay;

    void Awake()
    {
        CallHandler.CloudGoodsInitilized += CallHandler_CloudGoodsInitilized;
        CallHandler.Initialize();
    }

    void CallHandler_CloudGoodsInitilized()
    {
        CloudGoods.Services.AccountServices.Login(new LoginRequest("lionel.sy@gmail.com", "123456"), OnRegisteredtoSession);
    }

    void OnRegisteredtoSession(CloudGoodsUser user)
    {
        StoreDisplay.SetActive(true);
        itemBundlesLoader.GetItemBundles();
    }


}
