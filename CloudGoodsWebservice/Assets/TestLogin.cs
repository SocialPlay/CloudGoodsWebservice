using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLogin : MonoBehaviour {

    void Awake()
    {
        CloudGoods.Instance.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
    }

	// Use this for initialization
	void Start () {
        CloudGoods.Instance.Initialize();
	}

    void Instance_CloudGoodsInitilized()
    {
        CloudGoods.Instance.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);

        //CloudGoods.Instance.GetUserItems(OnReceivedUserItems);
    }

    void OnReceivedUserItems(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            Debug.Log("Item: " + item.Name);
        }
    }

    void OnReceivedUser(CloudGoodsUser user)
    {
        Debug.Log("User: " + user.userName);
        CloudGoods.Instance.GetUserItems(0, OnReceivedUserItems);
    }
}
