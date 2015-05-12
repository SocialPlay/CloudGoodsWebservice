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

        public void Login(LoginRequest request, Action<CloudGoodsUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginCallObject(request), x =>
            {
                callback(responseCreator.CreateLoginResponse(x));
            }));
        }

        public void Register(RegisterUserRequest request, Action<RegisteredUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateRegisterUserCallObject(request), x =>
            {
                callback(responseCreator.CreateRegisteredUserResponse(x));
            }
            ));
        }

        public void ForgotPassword(ForgotPasswordRequest request, Action<StatusMessageResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateForgotPasswordCallObject(request), x =>
            {
                callback(responseCreator.CreateStatusMessageResponse(x));
            }));
        }

        public void ResendVerificationEmail(ResendVerificationRequest request, Action<StatusMessageResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateResendVerificationEmailCallObject(request), x =>
                {
                    if (callback != null)
                        callback(responseCreator.CreateStatusMessageResponse(x));
                }));
        }

        public void LoginByPlatform(LoginByPlatformRequest request, Action<CloudGoodsUser> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateLoginByPlatformCallObject(request), x =>
            {
                callback(responseCreator.CreateLoginResponse(x));
            }));

        }
        #endregion

        #region Item Manipulation Services

        public void UserItems(UserItemsRequest request, Action<List<InstancedItemInformation>> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUserItemsCallObject(request), x =>
            {
                callback(responseCreator.CreateItemDataListResponse(x));
            }));
        }

        public void InstanceItems(InstanceItemsRequest request, Action<List<InstancedItemInformation>> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateInstanceItemsRequest(request), x =>
          {
              callback(responseCreator.CreateItemDataListResponse(x));
          }));
        }

        public void UserItem(OwnerItemRequest request, Action<SimpleItemInfo> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUserItemCall(request), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void MoveItems(MoveItemsRequest request, Action<UpdatedStacksResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemsCallObject(request), x =>
                {
                    callback(responseCreator.CreateUpdatedStacksResponse(x));
                }));
        }

        public void UpdateItemsByIds(UpdateItemsByIdsRequest request, Action<UpdatedStacksResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByIdRequestCallObject(request), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, AlternateDestinationOwner destinationOwner = null)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByStackIdRequestCallObject(new UpdateItemsByStackIdRequest() { Orders = orders, AlternateDestination = destinationOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void RedeemItemVoucher(RedeemItemVouchersRequest request, Action<UpdatedStacksResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateRedeemItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public void CreateItemVouchers(CreateItemVouchersRequest request, Action<ItemVouchersResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateCreateItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        public void ItemVoucher(ItemVoucherRequest request, Action<ItemVouchersResponse> callback)
        {
            StartCoroutine(ServiceGetString(callObjectCreator.CreateItemVoucherCall(request), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        #endregion

        #region Item Store Services

        public void GetCurrencyInfo(Action<CurrencyInfoResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateCurrencyInfoCall(new CurrencyInfoRequest()), x =>
            {
                callback(responseCreator.CreateCurrencyInfoResponse(x));
            }));
        }

        public void GetPremiumCurrencyBalance(Action<CurrencyBalanceResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePremiumCurrencyBalanceCall(new PremiumCurrencyBalanceRequest()), x =>
                {
                    if(callback != null)
                        callback(responseCreator.CreateCurrencyBalanceResponse(x));
                }));
        }

        public void GetStandardCurrencyBalance(StandardCurrencyBalanceRequest request, Action<SimpleItemInfo> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateStandardCurrencyBalanceCall(request), x =>
            {
                if(callback != null)
                    callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void PurchasePremiumCurrencyBundle(BundlePurchaseRequest request, Action<PurchasePremiumCurrencyBundleResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePremiumCurrencyBundlePurchaseCall(request), x =>
            {
                
            }));
        }

        public void ConsumePremiumCurrency(ConsumePremiumRequest request, Action<ConsumePremiumResponce> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateConsumePremiumCall(request), x =>
            {
                callback(responseCreator.CreateConsumePremiumResponce(x));
            }));
        }

        public void GetStoreItems(StoreItemsRequest request, Action<List<StoreItem>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateStoreItemsCall(request), x =>
            {
                callback(responseCreator.CreateGetStoreItemResponse(x));
            }));
        }

        public void PurchaseItem(PurchaseItemRequest request, Action<SimpleItemInfo> callback)
        {          
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePurchaseItemCall(request), x =>
            {
                callback(responseCreator.CreateSimpleItemInfoResponse(x));
            }));
        }

        public void GetItemBundles(ItemBundlesRequest request, Action<ItemBundlesResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateItemBundlesCall(request), x =>
            {
                callback(responseCreator.CreateItemBundlesResponse(x));
            }));
        }

        public void PurchaseItemBundle(ItemBundlePurchaseRequest request, Action<ItemBundlePurchaseResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.ItemBundlePurchaseCall(request), x =>
            {
                callback(responseCreator.CreateItemBundlePurchaseResponse(x));
            }));
        }

        public void PremiumBundles(PremiumBundlesRequest request, Action<List<PremiumCurrencyBundle>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePremiumCurrencyBundlesCall(request), x =>
                {
                    Debug.Log(x);
                    callback(responseCreator.CreatePremiumCurrencyBundleResponse(x));
                }));
        }

        #endregion

        #region Cloud Data Services

        public void GetUserData(UserDataRequest request, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataCall(request), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void UserDataUpdate(UserDataUpdateRequest request, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataUpdateCall(request), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void UserDataAll(Action<List<CloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataAllCall(new UserDataAllRequest()), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataListResponse(x));
            }));
        }

        public void UserDataByKey(UserDataByKeyRequest request, Action<List<OwnedCloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUserDataByKeyCall(request), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateUserDataByKeyResponse(x));
            }));
        }

        public void AppData(AppDataRequest request, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateAppDataCall(request), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataResponse(x));
            }));
        }

        public void AppDataAll(Action<List<CloudData>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateAppDataAllCall(new AppDataAllRequest()), x =>
            {
                if (callback != null)
                    callback(responseCreator.CreateCloudDataListResponse(x));
            }));
        }

        public void UpdateAppData(AppDataUpdateRequest request, Action<CloudData> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateAppDataUpdateCall(request), x =>
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
