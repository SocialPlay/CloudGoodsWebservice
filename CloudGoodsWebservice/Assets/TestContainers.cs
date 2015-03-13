using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestContainers : MonoBehaviour {

    public ItemContainer itemContainer;

    // Use this for initialization
    void Start()
    {
        CloudGoods.Initialize();
        CloudGoods.CloudGoodsInitilized += Instance_CloudGoodsInitilized;
    }

    void Instance_CloudGoodsInitilized()
    {
        CloudGoods.Login(CloudGoodsPlatform.SocialPlay, "0", "lionel.sy@gmail.com", "123456", OnReceivedUser);
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
        foreach (PersistentItemContainer loader in GameObject.FindObjectsOfType(typeof(PersistentItemContainer)))
        {

                loader.transform.parent.GetComponent<ItemContainer>().Clear();
                loader.LoadItems();
           
        }
    }

    // Update is called once per frame
    void Update()
    {
	
	}
}
