using UnityEngine;
using System.Collections;
using System;

public class CloudGoods : MonoBehaviour {

    private static CloudGoods _instance;

    public static CloudGoods Instance()
    {
        if (_instance == null)
        {
            GameObject cloudGoodsObject = new GameObject("_CloudGoods");
            cloudGoodsObject.AddComponent<CloudGoods>();
            _instance = cloudGoodsObject.GetComponent<CloudGoods>();
        }

        return _instance;
    }

    public string AppID = "ae3af4a3-de7e-4d59-818e-10b11030df1e";
    string UserID = "";

    CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
    ResponseCreator responseCreator = new LitJsonResponseCreator();

    public void Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        Instance().StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(AppID, userEmail, password), x => {
            callback(responseCreator.CreateLoginResponse(x));
        }));
    }

    IEnumerator ServiceGetString(WWW www, Action<string> callback)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            callback(www.text);
        }
        else
        {
            Debug.Log(www.text);
            Debug.LogError("Error: " + www.error);
        }
    }
}

public enum CloudGoodsPlatform
{
    Facebook = 1,
    SocialPlay = 2,
    Google = 3,
    Kongregate = 4,
    Custom = 5
}
