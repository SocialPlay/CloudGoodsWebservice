using UnityEngine;
using System.Collections;
using System;

public class CloudGoods : MonoBehaviour {

    #region public variables

    public string AppID = "ae3af4a3-de7e-4d59-818e-10b11030df1e";
    public string appSecret = "K6bFGgvtetjVIZHplj5icXJQAqhPCL";
    public int ServerTimeDifference = 0;

    public event Action CloudGoodsInitilized;

    #endregion

    #region private variables

    CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
    ResponseCreator responseCreator = new LitJsonResponseCreator();

    bool isInitialized = false;

    #endregion

    #region Initialize

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

    public void Initialize()
    {
        GetServerTime();
    }

    #endregion

    #region UserManagement

    public void Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance().StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(AppID, userEmail, password), x => {
            callback(responseCreator.CreateLoginResponse(x));
        }));
    }

    #endregion 


    #region Server Utilities

    void GetServerTime()
    {
        Instance().StartCoroutine(ServiceGetString(callObjectCreator.CreateGetServerTimeObject(), x =>
        {
            Debug.Log("Server Time stamp: " + x);
            CalculateServerClientTimeDifference(int.Parse(x));
            isInitialized = true;
            CloudGoodsInitilized();
        }));
    }

    void CalculateServerClientTimeDifference(int serverTime)
    {
        ServerTimeDifference = DateTime.UtcNow.ConvertToUnixTimestamp() - serverTime;

        Debug.Log("difference: " + ServerTimeDifference + " now time: " + DateTime.Now.ToString());
    }

    #endregion

    #region Coroutines

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

    #endregion

    #region Utilities


    #endregion
}

public enum CloudGoodsPlatform
{
    Facebook = 1,
    SocialPlay = 2,
    Google = 3,
    Kongregate = 4,
    Custom = 5
}

public static class Utilities
{
    public static int ConvertToUnixTimestamp(this DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return (int)Math.Floor(diff.TotalSeconds);
    }
}
