using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;

public class CloudGoods : MonoBehaviour {

    #region public variables

    public string SessionId = "";
    public int ServerTimeDifference = 0;

    public event Action CloudGoodsInitilized;

    public CloudGoodsUser User;

    #endregion

    #region private variables

    CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
    ResponseCreator responseCreator = new LitJsonResponseCreator();

    bool isInitialized = false;

    #endregion

    #region Initialize

    private static CloudGoods _instance;

    public static CloudGoods Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject cloudGoodsObject = new GameObject("_CloudGoods");
                cloudGoodsObject.AddComponent<CloudGoods>();
                _instance = cloudGoodsObject.GetComponent<CloudGoods>();
            }


            return _instance;
        }
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

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(CloudGoodsSettings.AppID, userEmail, password), x => {
            User = responseCreator.CreateLoginResponse(x);
            SessionId = User.sessionID;
            callback(User);
        }));
    }

    #endregion 

    #region ItemManagement

    public void GetUserItems(int location, Action<List<ItemData>> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetUserItemsCallObject(location), x =>
        {
            callback(responseCreator.CreateGetUserItemsResponse(x));
        }));
    }

    public void MoveItem(ItemData item, int location, int amountToMove, Action<NewItemStack> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemCallObject(item.StackLocationId, amountToMove, location, "User"), x =>
            {
                callback(responseCreator.CreateMoveItemResponse(x));
            })); 
    }

    #endregion

    #region Coroutines

    IEnumerator ServiceGetString(WWW www, Action<string> callback)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            ValidateData(www);
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

    private void ValidateData(WWW www)
    {
        string responseData = www.text;
        if (!responseCreator.IsValidData(responseData)) { };
        if (responseCreator.IsWebserviceError(responseData)) { };
    }

    void GetServerTime()
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetServerTimeObject(), x =>
        {
            Debug.Log("Server Time stamp: " + x);
            CalculateServerClientTimeDifference(int.Parse(x));
            isInitialized = true;

            if (CloudGoodsInitilized != null) 
                CloudGoodsInitilized();
        }));
    }

    void CalculateServerClientTimeDifference(int serverTime)
    {
        ServerTimeDifference = DateTime.UtcNow.ConvertToUnixTimestamp() - serverTime;

        Debug.Log("difference: " + ServerTimeDifference + " now time: " + DateTime.Now.ToString());
    }

    #endregion
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
