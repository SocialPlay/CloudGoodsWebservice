using UnityEngine;
using System.Collections;

public class TestLogin : MonoBehaviour {

    void Awake()
    {

    }

	void Start () {
        CloudGoods.Instance().Login(CloudGoodsPlatform.SocialPlay, "", "lionelsy", "lionel.sy@gmail.com", "123456", Callback);
	}

    void Callback(UserResponse response)
    {
        Debug.Log("Response: " + response.message);
    }
}
