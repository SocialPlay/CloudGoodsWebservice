using UnityEngine;
using System.Collections;

public class TestLogin : MonoBehaviour {

    void Awake()
    {
        CloudGoods.Instance().CloudGoodsInitilized += TestLogin_CloudGoodsInitilized;
    }

    void Start()
    {
        CloudGoods.Instance().Initialize();
    }


    void TestLogin_CloudGoodsInitilized()
    {
        CloudGoods.Instance().Login(CloudGoodsPlatform.SocialPlay, "", "lionel.sy@gmail.com", "123456", Callback);
    }

    void Callback(CloudGoodsUser response)
    {
        Debug.Log("cloud goods user session: " + response.sessionID + " userID: " + response.UserID + " user email: " + response.userEmail + " username: " + response.userName);
    }
}
