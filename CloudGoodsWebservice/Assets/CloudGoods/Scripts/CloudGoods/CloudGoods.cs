using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;
using CloudgoodsClasses;
using CloudGoodsUtilities;

public class CloudGoods : MonoBehaviour
{

    public static string SessionId = "";
    public static int ServerTimeDifference = 0;
    public static event Action CloudGoodsInitilized;
    public static CloudGoodsUser User;

    CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
    ResponseCreator responseCreator = new LitJsonResponseCreator();
    bool isInitialized = false;

    #region Initialize

    private static CloudGoods _instance;

    private static CloudGoods Instance
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

    public static void Initialize()
    {
        Instance.GetServerTime();
    }

    #endregion



    public static void Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        Instance._Login(cloudGoodsPlatform, platformUserID, userEmail, password, callback);
    }

    private void _Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(CloudGoodsSettings.AppID, userEmail, password), x =>
        {
            User = responseCreator.CreateLoginResponse(x);
            SessionId = User.sessionID;
            callback(User);
        }));
    }

    public static void GetUserItems(int location, Action<List<ItemData>> callback)
    {
        Instance._GetUserItems(location, callback);
    }
    private void _GetUserItems(int location, Action<List<ItemData>> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetOwnerItemsCallObject(location), x =>
        {
            callback(responseCreator.CreateGetUserItemsResponse(x));
        }));
    }

    public static void MoveItem(ItemData item, int location, int amountToMove, Action<NewItemStack> callback)
    {
        Instance._MoveItem(item, location, amountToMove, callback);
    }

    private void _MoveItem(ItemData item, int location, int amountToMove, Action<NewItemStack> callback)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemCallObject(item.StackLocationId, amountToMove, location, "User"), x =>
            {
                callback(responseCreator.CreateMoveItemResponse(x));
            }));
    }

    public static void GiveOwnerItems(GiveOwnerItemRequest.ItemInfo[] infos, Action<GiveOwnerItemResponse> callback, OtherOwner otherOwner = null)
    {
        Instance._GiveOwnerItems(infos, callback, otherOwner);
    }

    private void _GiveOwnerItems(GiveOwnerItemRequest.ItemInfo[] infos, Action<GiveOwnerItemResponse> callback, OtherOwner otherOwner = null)
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGiveOwnerItemsCallObject(new GiveOwnerItemRequest() { items = infos, otherOwner = otherOwner }), x =>
        {
            callback(responseCreator.CreateGiveOwnerItemResponse(x));
        }));
    }


    public static void ConsumeItemVoucher(List<ConsumeItemVouchersRequest.ItemVoucherSelection> selections, Action<ConsumeItemVouchersResponse> callback, OtherOwner otherOwner = null) //ToDo: Add callback
    {
        Instance._ConsumeItemVoucher(selections, callback, otherOwner);
    }


    private void _ConsumeItemVoucher(List<ConsumeItemVouchersRequest.ItemVoucherSelection> selections, Action<ConsumeItemVouchersResponse> callback, OtherOwner otherOwner = null)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        CloudgoodsClasses.ConsumeItemVouchersRequest request = new CloudgoodsClasses.ConsumeItemVouchersRequest()
        {
            selectedVouchers = selections,
            otherOwner = otherOwner
        };

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateConsumeItemVouchersCall(request), x =>
        {
            callback(responseCreator.CreteConsomeItemVoucherResponse(x));
        }));
    }

    public static void CreateItemVouchers(int minEnergy, int total, Action<CreateItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
    {
        Instance._CreateItemVouchers(minEnergy, total, callback, andTags, orTags);
    }

    private void _CreateItemVouchers(int minEnergy, int total, Action<CreateItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
    {
        if (!isInitialized)
            throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Instance().Initialize() first");

        CloudgoodsClasses.CreateItemVouchersRequest request = new CreateItemVouchersRequest()
        {
            minimumEnergy = minEnergy,
            totalEnergy = total,
            andTags = andTags.ToCommaSeparated(),
            orTags = orTags.ToCommaSeparated()
        };

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateCreateItemVouchersCall(request), x =>
        {
            callback(responseCreator.CreateCreateItemVoucherResponse(x));
        }));
    }


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
            Debug.LogError("Error: " + www.url);
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

namespace CloudGoodsUtilities
{

    public static class Utilities
    {
        public static int ConvertToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (int)Math.Floor(diff.TotalSeconds);
        }

        public static string ToCommaSeparated(this List<string> array)
        {
            if (array == null) return string.Empty;
            string results = "";
            array.ForEach(s => results += s + ',');
            results.TrimEnd(',');
            return results;
        }
    }
}
