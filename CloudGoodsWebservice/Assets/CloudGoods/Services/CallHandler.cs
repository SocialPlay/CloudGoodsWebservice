using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;
using CloudGoods.SDK.Models;
using CloudGoods.Enums;
using CloudGoodsUtilities;
using CloudGoods.Services.Webservice;
using CloudGoods.SDK.Utilities;


namespace CloudGoods.Services.WebCommunication
{
    public class CallHandler : MonoBehaviour
    {
        public static event Action<WebserviceError> IsError;

        static public event Action CloudGoodsInitilized;
        static public event Action<string> onErrorEvent;

        static bool isGettingWorldInfo = false;

        public static string SessionId = "";
        public static int ServerTimeDifference = 0;

        CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
        ResponseCreator responseCreator = new LitJsonResponseCreator();
        public static bool isInitialized = false;

        #region Initialize

        private static CallHandler _instance;

        public static CallHandler Instance
        {
            get
            {
                if (!isInitialized)
                    throw new Exception("Cloud Goods has not yet been initialized. Before making any webservice calls with the CloudGoods class, you must call CloudGoods.Initialize() first");
                return GetInstance();
            }
        }

        private static CallHandler GetInstance()
        {
            if (_instance == null)
            {
                GameObject cloudGoodsObject = new GameObject("_CloudGoods");
                cloudGoodsObject.AddComponent<CallHandler>();
                _instance = cloudGoodsObject.GetComponent<CallHandler>();
            }
            return _instance;
        }

        public static void Initialize()
        {
            GetServerTime(GetInstance());
        }

        #endregion

        #region AccountManagemnt

        public void Login(string userEmail, string password, Action<CloudGoodsUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(userEmail, password), x =>
            {
                callback(responseCreator.CreateLoginResponse(x));
            }));
        }

        public void Register(string appId, string userName, string userEmail, string password, Action<RegisteredUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateRegisterUserCallObject(new RegisterUserRequest()
            {
                AppId = appId,
                UserName = userName,
                UserEmail = userEmail,
                Password = password
            }), x =>
            {
                callback(responseCreator.CreateRegisteredUserResponse(x));
            }
            ));
        }

        public void ForgotPassword(string userEmail, Action<StatusMessageResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateForgotPasswordCallObject(userEmail), x =>
            {
                callback(responseCreator.CreateStatusMessageResponse(x));
            }));
        }

        public void ResendVerificationEmail(string email, Action<StatusMessageResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateResendVerificationEmailCallObject(email), x =>
                {
                    if (callback != null)
                        callback(responseCreator.CreateStatusMessageResponse(x));
                }));
        }

        public void LoginByPlatform(string userName, CloudGoodsPlatform cloudGoodsPlatform, string platformUserID, Action<CloudGoodsUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginByPlatformCallObject(userName, (int)cloudGoodsPlatform, platformUserID), x =>
            {
                callback(responseCreator.CreateLoginResponse(x));
            }));

        }
        #endregion

        #region Item Manipulation Services

        public void GetUserItems(int location, Action<List<InstancedItemInformation>> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateGetUserItemsCallObject(location), x =>
            {
                callback(responseCreator.CreateItemDataListResponse(x));
            }));
        }

        public void UserItem(int lookupItemId, int location, Action<SimpleItemInfo> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUserItemCall(lookupItemId, location), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void MoveItems(List<MoveItemsRequest.MoveOrder> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemsCallObject(new MoveItemsRequest() { MoveOrders = orders, OtherOwner = otherOwner }), x =>
                {
                    callback(responseCreator.CreateUpdatedStacksResponse(x));
                }));
        }

        public void UpdateItemsByIds(List<UpdateItemByIdRequest.UpdateOrderByID> orders, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByIdRequestCallObject(new UpdateItemByIdRequest() { Orders = orders, OtherOwner = otherOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, OtherOwner destinationOwner = null)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByStackIdRequestCallObject(new UpdateItemsByStackIdRequest() { Orders = orders, DestinationOwner = destinationOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void RedeemItemVoucher(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            RedeemItemVouchersRequest request = new RedeemItemVouchersRequest()
                {
                    SelectedVouchers = selections,
                    OtherOwner = otherOwner
                };

            StartCoroutine(ServiceGetString(callObjectCreator.CreateRedeemItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void CreateItemVouchers(int minEnergy, int total, Action<ItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
        {
            CloudGoods.SDK.Models.CreateItemVouchersRequest request = new CreateItemVouchersRequest()
            {
                MinimumEnergy = minEnergy,
                TotalEnergy = total,
                AndTags = andTags.ToCommaSeparated(),
                OrTags = orTags.ToCommaSeparated()
            };
            StartCoroutine(ServiceGetString(callObjectCreator.CreateCreateItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        public void GetItemVoucher(int voucherId, Action<ItemVouchersResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateItemVoucherCall(voucherId), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        #endregion

        #region Item Store Services

        public void GetCurrencyInfo(Action<CurrencyInfoResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateCurrencyInfoCall(), x =>
            {
                callback(responseCreator.CreateCurrencyInfoResponse(x));
            }));
        }

        public void GetPremiumCurrencyBalance(Action<CurrencyBalanceResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePremiumCurrencyBalanceCall(), x =>
                {
                    callback(responseCreator.CreateCurrencyBalanceResponse(x));
                }));
        }

        public void GetStandardCurrencyBalance(int accessLocation, Action<SimpleItemInfo> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateStandardCurrencyBalanceCall(accessLocation), x =>
            {
                callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void ConsumePremiumCurrency(int amount, Action<ConsumePremiumResponce> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateConsumePremiumCall(new ConsumePremiumRequest() { Amount = amount }), x =>
            {
                callback(responseCreator.CreateConsumePremiumResponce(x));
            }));
        }

        public void GetStoreItems(string andTags, string orTags, Action<List<StoreItem>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetStoreItemsCall(andTags, orTags), x =>
            {
                callback(responseCreator.CreateGetStoreItemResponse(x));
            }));
        }

        public void PurchaseItem(int itemId, int amount, int paymentOption, int saveLocation, Action<SimpleItemInfo> callback, int amountToConsume = -1)
        {
            PurchaseItemRequest request = new PurchaseItemRequest()
            {
                ItemId = itemId,
                BuyAmount = amount,
                PaymentOption = (PurchaseItemRequest.PaymentType)paymentOption,
                SaveLocation = saveLocation,
                Consume = amountToConsume == -1 ? null : new PurchaseItemRequest.ConsumeUponPurchase() { Amount = amountToConsume }
            };
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePurchaseItemCall(request), x =>
            {
                callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void GetItemBundles(string andTags, string orTags, Action<ItemBundlesResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateItemBundlesCall(andTags, orTags), x =>
            {
                callback(responseCreator.CreateItemBundlesResponse(x));
            }));
        }

        public void PurchaseItemBundle(int bundleId, int paymentType, int location, Action<ItemBundlePurchaseResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.ItemBundlePurchaseCall(new ItemBundlePurchaseRequest() { BundleID = bundleId, PaymentType = paymentType, Location = location }), x =>
            {
                callback(responseCreator.CreateItemBundlePurchaseResponse(x));
            }));
        }

        public void GetPremiumBundles(int platformId, Action<List<PremiumCurrencyBundle>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetPremiumCurrencyBundlesCall(platformId), x =>
                {
                    callback(responseCreator.CreatePremiumCurrencyBundleResponse(x));
                }));
        }

        #endregion

        #region Cloud Data Services

        public void GetUserData(string key, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataCall(key), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void UserDataUpdate(string key, string value, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataUpdateCall(key, value), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void UserDataAll(Action<List<CloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataAllCall(), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataListResponse(x));
            }));
        }

        public void UserDataByKey(string key, Action<List<OwnedCloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataByKeyCall(key), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateUserDataByKeyResponse(x));
            }));
        }

        public void AppData(string key, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateAppDataCall(key), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void AppDataAll(Action<List<CloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateAppDataAllCall(), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataListResponse(x));
            }));
        }

        public void UpdateAppData(string key, string value, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateAppDataCall(key, value), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
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
                if (ValidateData(www))
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

        private bool ValidateData(WWW www)
        {
            string responseData = www.text;
            WebserviceError error = responseCreator.IsWebserviceError(responseData);
            if (error != null)
            {
                Debug.LogError("Error: " + error.Message);

                if (IsError != null)
                {
                    IsError(error);
                }

                return false;
            };
            if (!responseCreator.IsValidData(responseData))
            {
                return false;
            };

            return true;

        }

        static void GetServerTime(CallHandler cg)
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
