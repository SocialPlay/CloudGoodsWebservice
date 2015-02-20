using UnityEngine;
using System.Collections;

public class TestLogin : MonoBehaviour {

	void Start () {
        CloudGoods.Instance().Login(CloudGoodsPlatform.SocialPlay, "", "lionel.sy@gmail.com", "123456", Callback);
	}

    void Callback(CloudGoodsUser response)
    {
        Debug.Log("cloud goods user session: " + response.sessionID + " userID: " + response.UserID + " user email: " + response.userEmail + " username: " + response.userName);
    }
}
