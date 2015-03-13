using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLogin : MonoBehaviour {

    void Awake()
    {
        CloudGoods.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
    }

	// Use this for initialization
	void Start () {
        CloudGoods.Initialize();
	}

    void Instance_CloudGoodsInitilized()
    {
        CloudGoods.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

        //CloudGoods.Instance.GetUserItems(OnReceivedUserItems);
    }

    void OnReceivedUserItems(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            Debug.Log("Item: " + item.name);
        }
    }

    void OnReceivedUser(CloudGoodsUser user)
    {
        Debug.Log("User: " + user.userName);
        CloudGoods.GetUserItems(0, OnReceivedUserItems);
    }
}
