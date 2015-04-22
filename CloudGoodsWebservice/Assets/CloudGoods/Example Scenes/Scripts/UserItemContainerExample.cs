using UnityEngine;
using System.Collections;
using CloudGoods.Services.WebCommunication;
using CloudGoods.Services;
using CloudGoods.SDK.Models;
using System.Collections.Generic;
using CloudGoods.SDK.Container;

public class UserItemContainerExample : MonoBehaviour {


    public List<PersistentItemContainer> containers;

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
        foreach(PersistentItemContainer container in containers)
        {
            container.LoadItems();
        }
    }
}
