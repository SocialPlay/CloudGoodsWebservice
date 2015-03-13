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
    static bool isInitialized = false;

    #region Initialize

    private static CloudGoods _instance;

    private static CloudGoods Instance
    {
        get
        {
            if (!isInitialized)
                throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Initialize() first");
            return GetInstance();
        }
    }

    private static CloudGoods GetInstance()
    {
        if (_instance == null)
        {
            GameObject cloudGoodsObject = new GameObject("_CloudGoods");
            cloudGoodsObject.AddComponent<CloudGoods>();
            _instance = cloudGoodsObject.GetComponent<CloudGoods>();
        }
        return _instance;
    }

    public static void Initialize()
    {
        GetServerTime(GetInstance());
    }

    #endregion

    public static void Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        Instance._Login(cloudGoodsPlatform, platformUserID, userEmail, password, callback);
    }

    private void _Login(CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, string userEmail, string password, Action<CloudGoodsUser> callback)
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(CloudGoodsSettings.AppID, userEmail, password), x =>
        {
            User = responseCreator.CreateLoginResponse(x);
            SessionId = User.SessionID;
            callback(User);
        }));
    }

    public static void GetUserItems(int location, Action<List<ItemData>> callback)
    {
        Instance._GetUserItems(location, callback);
    }
    private void _GetUserItems(int location, Action<List<ItemData>> callback)
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetUserItemsCallObject(location), x =>
        {
            callback(responseCreator.CreateItemDataListResponse(x));
        }));
    }

    public static void MoveItem(ItemData item, int location, int amountToMove, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        List<MoveItemsRequest.MoveOrder> orders = new List<MoveItemsRequest.MoveOrder>(){
             new MoveItemsRequest.MoveOrder(){
                     stackId = item.StackLocationId,
         amount = amountToMove,
           location =location
             }
        };
        Instance._MoveItems(orders, callback);
    }

    public static void MoveItems(List<MoveItemsRequest.MoveOrder> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        Instance._MoveItems(orders, callback);
    }

    private void _MoveItems(List<MoveItemsRequest.MoveOrder> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemsCallObject(new MoveItemsRequest() { moveOrders = orders, otherOwner = otherOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
    }

    public static void UpdateItemById(int itemId, int amount, int location, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        List<UpdateItemByIdRequest.UpdateOrderByID> orders = new List<UpdateItemByIdRequest.UpdateOrderByID>(){
            new UpdateItemByIdRequest.UpdateOrderByID(){
                itemId = itemId,
                amount = amount,
                location = location
            }
        };
        Instance._UpdateItemsByIds(orders, callback, otherOwner);
    }

    public static void UpdateItemsByIds(List<UpdateItemByIdRequest.UpdateOrderByID> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        Instance._UpdateItemsByIds(orders, callback, otherOwner);
    }

    private void _UpdateItemsByIds(List<UpdateItemByIdRequest.UpdateOrderByID> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
    {
        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByIdRequestCallObject(new UpdateItemByIdRequest() { orders = orders, otherOwner = otherOwner }), x =>
        {
            callback(responseCreator.CreateUpdatedStacksResponse(x));
        }));
    }


    public static void RedeemItemVouchers(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<RedeemItemVouchersResponse> callback, OtherOwner otherOwner = null) //ToDo: Add callback
    {
        Instance._RedeemItemVoucher(selections, callback, otherOwner);
    }


    private void _RedeemItemVoucher(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<RedeemItemVouchersResponse> callback, OtherOwner otherOwner = null)
    {
        CloudgoodsClasses.RedeemItemVouchersRequest request = new CloudgoodsClasses.RedeemItemVouchersRequest()
            {
                SelectedVouchers = selections,
                OtherOwner = otherOwner
            };

        Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateRedeemItemVouchersCall(request), x =>
        {
            callback(responseCreator.CreteRedeemItemVoucherResponse(x));
        }));
    }

    public static void CreateItemVouchers(int minEnergy, int total, Action<CreateItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
    {
        Instance._CreateItemVouchers(minEnergy, total, callback, andTags, orTags);
    }

    private void _CreateItemVouchers(int minEnergy, int total, Action<CreateItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
    {
        CloudgoodsClasses.CreateItemVouchersRequest request = new CreateItemVouchersRequest()
        {
            MinimumEnergy = minEnergy,
            TotalEnergy = total,
            AndTags = andTags.ToCommaSeparated(),
            OrTags = orTags.ToCommaSeparated()
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

    static void GetServerTime(CloudGoods cg)
    {
        cg.StartCoroutine(cg.ServiceGetString(cg.callObjectCreator.CreateGetServerTimeObject(), x =>
        {
            cg.CalculateServerClientTimeDifference(int.Parse(x));
            isInitialized = true;

            if (CloudGoodsInitilized != null)
                CloudGoodsInitilized();
        }));
    }

    void CalculateServerClientTimeDifference(int serverTime)
    {
        ServerTimeDifference = DateTime.UtcNow.ConvertToUnixTimestamp() - serverTime;
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
