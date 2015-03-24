using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;
using CloudGoods.Models;
using CloudGoods.Enums;
using CloudGoodsUtilities;
using CloudGoods.Webservice;
using CloudGoods.Utilities;

namespace CloudGoods
{
    public class CallHandler : MonoBehaviour
    {
        public static event Action<WebserviceError> IsError;

        static public event Action CloudGoodsInitilized;
        static public event Action<string> onErrorEvent;
        static public event Action onLogout;
        static public event Action<UserResponse> OnUserRegister;
        static public event Action<UserResponse> OnForgotPassword;
        static public event Action<UserResponse> OnVerificationSent;
        static public event Action<string> OnRegisteredUserToSession;
        static public event Action<CloudGoodsUser> OnUserAuthorized;
        static public event Action<List<StoreItem>> OnStoreListLoaded;
        //static public event Action<List<ItemBundle>> OnStoreItemBundleListLoaded;
        //static public event Action<List<ItemData>> OnItemsLoaded;
        static public event Action<int> OnStandardCurrency;
        static public event Action<int> OnPremiumCurrency;
        static public event Action<string> OnStandardCurrencyName;
        static public event Action<string> OnPremiumCurrencyName;
        static public event Action<Texture2D> OnStandardCurrencyTexture;
        static public event Action<Texture2D> OnPremiumCurrencyTexture;

        static public int StandardCurrency
        {
            get { return mFree; }
            private set
            {
                if (mFree != value)
                {
                    mFree = value;
                    if (OnStandardCurrency != null) OnStandardCurrency(mFree);
                }
            }
        }
        static public int PremiumCurrency
        {
            get { return mPaid; }
            private set
            {
                if (mPaid != value)
                {
                    mPaid = value;
                    if (OnPremiumCurrency != null) OnPremiumCurrency(mPaid);
                }
            }
        }
        static public Texture2D standardCurrencyTexture
        {
            get
            {
                if (tFree != null)
                    return tFree;
                else
                {
                    if (isGettingWorldInfo == false)
                    {
                        GetCurrencyInfo(null);
                        isGettingWorldInfo = true;
                    }

                    return null;
                }
            }
        }
        static public Texture2D premiumCurrencyTexture
        {
            get
            {
                if (tPaid != null)
                    return tPaid;
                else
                {
                    if (isGettingWorldInfo == false)
                    {
                        GetCurrencyInfo(null);
                        isGettingWorldInfo = true;
                    }

                    return null;
                }
            }
        }
        static public string StandardCurrencyName
        {
            get
            {
                if (!string.IsNullOrEmpty(sfree))
                {
                    return sfree;
                }
                else
                {
                    if (isGettingWorldInfo == false)
                    {
                        GetCurrencyInfo(null);
                        isGettingWorldInfo = true;
                    }

                    return null;
                }
            }

        }
        static public string PremiumCurrencyName
        {
            get
            {
                if (!string.IsNullOrEmpty(sPaid))
                {
                    return sPaid;
                }
                else
                {
                    if (isGettingWorldInfo == false)
                    {
                        GetCurrencyInfo(null);
                        isGettingWorldInfo = true;
                    }

                    return null;
                }
            }
        }

        static bool isGettingWorldInfo = false;
        static int mFree = 0;
        static int mPaid = 0;
        static Texture2D tFree;
        static Texture2D tPaid;
        static string sfree;
        static string sPaid;

        public static string SessionId = "";
        public static int ServerTimeDifference = 0;
        public static CloudGoodsUser User;

        CallObjectCreator callObjectCreator = new WebAPICallObjectCreator();
        ResponseCreator responseCreator = new LitJsonResponseCreator();
        static bool isInitialized = false;

        #region Initialize

        private static CallHandler _instance;

        private static CallHandler Instance
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

        #region Items
        public static void GetUserItems(int location, Action<List<InstancedItemInformation>> callback)
        {
            Instance._GetUserItems(location, callback);
        }

        private void _GetUserItems(int location, Action<List<InstancedItemInformation>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetUserItemsCallObject(location), x =>
            {
                callback(responseCreator.CreateItemDataListResponse(x));
            }));
        }

        public static void MoveItem(OwnedItemInformation item, int location, int amountToMove, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            List<MoveItemsRequest.MoveOrder> orders = new List<MoveItemsRequest.MoveOrder>(){
             new MoveItemsRequest.MoveOrder(){
                     StackId = item.StackLocationId,
         Amount = amountToMove,
           Location =location
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
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateMoveItemsCallObject(new MoveItemsRequest() { MoveOrders = orders, OtherOwner = otherOwner }), x =>
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
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByIdRequestCallObject(new UpdateItemByIdRequest() { Orders = orders, OtherOwner = otherOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public static void UpdateItemByStackIds(string stackId, int amount, int location, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders = new List<UpdateItemsByStackIdRequest.UpdateOrderByStackId>(){
            new UpdateItemsByStackIdRequest.UpdateOrderByStackId(){
                stackId = stackId,
                amount = amount,
                location = location
            }
        };
            Instance._UpdateItemByStackIds(orders, callback, otherOwner);
        }

        public static void UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, OtherOwner destinationOwner = null)
        {
            Instance._UpdateItemByStackIds(orders, callback, destinationOwner);
        }

        private void _UpdateItemByStackIds(List<UpdateItemsByStackIdRequest.UpdateOrderByStackId> orders, Action<UpdatedStacksResponse> callback, OtherOwner destinationOwner = null)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateUpdateItemByStackIdRequestCallObject(new UpdateItemsByStackIdRequest() { Orders = orders, DestinationOwner = destinationOwner }), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }
        #endregion

        #region Item Voucher

        public static void RedeemItemVouchers(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null) //ToDo: Add callback
        {
            Instance._RedeemItemVoucher(selections, callback, otherOwner);
        }

        private void _RedeemItemVoucher(List<RedeemItemVouchersRequest.ItemVoucherSelection> selections, Action<UpdatedStacksResponse> callback, OtherOwner otherOwner = null)
        {
            RedeemItemVouchersRequest request = new RedeemItemVouchersRequest()
                {
                    SelectedVouchers = selections,
                    OtherOwner = otherOwner
                };

            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateRedeemItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateUpdatedStacksResponse(x));
            }));
        }

        public static void CreateItemVouchers(int minEnergy, int total, Action<ItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
        {
            Instance._CreateItemVouchers(minEnergy, total, callback, andTags, orTags);
        }

        private void _CreateItemVouchers(int minEnergy, int total, Action<ItemVouchersResponse> callback, List<string> andTags = null, List<string> orTags = null)
        {
            CloudGoods.Models.CreateItemVouchersRequest request = new CreateItemVouchersRequest()
            {
                MinimumEnergy = minEnergy,
                TotalEnergy = total,
                AndTags = andTags.ToCommaSeparated(),
                OrTags = orTags.ToCommaSeparated()
            };

            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateCreateItemVouchersCall(request), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        public static void GetItemVoucher(int voucherId, Action<ItemVouchersResponse> callback)
        {
            Instance._GetItemVoucher(voucherId, callback);
        }

        private void _GetItemVoucher(int voucherId, Action<ItemVouchersResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateItemVoucherCall(voucherId), x =>
            {
                callback(responseCreator.CreateItemVoucherResponse(x));
            }));
        }

        #endregion

        #region Store

        public static void GetCurrencyInfo(Action<CurrencyInfoResponse> callback)
        {
            Instance._GetCurrencyInfo(callback);
        }

        private void _GetCurrencyInfo(Action<CurrencyInfoResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateCurrencyInfoCall(), x =>
                {
                    CurrencyInfoResponse response = responseCreator.CreateCurrencyInfoResponse(x);

                    if (!string.IsNullOrEmpty(response.StandardCurrencyName))
                    {
                        sfree = response.StandardCurrencyName;
                        if (OnStandardCurrencyName != null) OnStandardCurrencyName(sPaid);
                    }

                    if (!string.IsNullOrEmpty(response.PremiumCurrencyName))
                    {
                        sPaid = response.PremiumCurrencyName;
                        if (OnPremiumCurrencyName != null) OnPremiumCurrencyName(sPaid);
                    }

                    if (!string.IsNullOrEmpty(response.PremiumCurrencyImage))
                    {
                        ItemTextureCache.Instance.GetItemTexture(response.PremiumCurrencyImage, delegate(ImageStatus imageStatus, Texture2D texture)
                        {
                            tPaid = texture;
                            if (OnPremiumCurrencyTexture != null) OnPremiumCurrencyTexture(texture);
                        });
                    }

                    if (!string.IsNullOrEmpty(response.StandardCurrencyImage))
                    {
                        ItemTextureCache.Instance.GetItemTexture(response.StandardCurrencyImage, delegate(ImageStatus imageStatus, Texture2D texture)
                        {
                            tFree = texture;
                            if (OnStandardCurrencyTexture != null) OnStandardCurrencyTexture(texture);
                        });
                    }

                    if (callback != null)
                    callback(responseCreator.CreateCurrencyInfoResponse(x));
                }));
        }

        public static void GetPremiumCurrencyBalance(Action<CurrencyBalanceResponse> callback)
        {
            Instance._GetPremiumCurrencyBalance(callback);
        }

        private void _GetPremiumCurrencyBalance(Action<CurrencyBalanceResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreatePremiumCurrencyBalanceCall(), x =>
                {
                    CurrencyBalanceResponse balanceResponse = responseCreator.CreateCurrencyBalanceResponse(x);
                    PremiumCurrency = balanceResponse.Amount;

                    if(OnPremiumCurrency != null)
                        CallHandler.OnPremiumCurrency(balanceResponse.Amount);

                    if (callback != null)
                        callback(balanceResponse);
                        
                }));
        }

        public static void GetStandardCurrencyBalance(int accessLocation, Action<SimpleItemInfo> callback)
        {
            Instance._GetStandardCurrencyBalance(accessLocation, callback);
        }

        private void _GetStandardCurrencyBalance(int accessLocation, Action<SimpleItemInfo> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateStandardCurrencyBalanceCall(accessLocation), x =>
            {
                SimpleItemInfo itemInfo = responseCreator.CreateSimpleItemInfoResponse(x);
                StandardCurrency = itemInfo.Amount;

                if(OnStandardCurrency != null)
                    CallHandler.OnStandardCurrency(itemInfo.Amount);

                if (callback != null)
                    callback(itemInfo);
            }));
        }

        public static void GetStoreItems(Action<List<StoreItem>> callback, string andTags = null, string orTags = null)
        {
            Instance._GetStoreItems(andTags, orTags, callback);
        }

        private void _GetStoreItems(string andTags, string orTags, Action<List<StoreItem>> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateGetStoreItemsCall(andTags, orTags), x =>
            {
                callback(responseCreator.CreateGetStoreItemResponse(x));
            }));
        }

        public static void GetItemBundles(string andTags, string orTags, Action<ItemBundlesResponse> callback)
        {
            Instance._GetItemBundles(andTags, orTags, callback);
        }

        private void _GetItemBundles(string andTags, string orTags, Action<ItemBundlesResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.CreateItemBundlesCall(andTags, orTags), x =>
            {
                callback(responseCreator.CreateItemBundlesResponse(x));
            }));
        }
        public static void PurchaseItemBundle(int bundleId, int paymentType, int location, Action<ItemBundlePurchaseResponse> callback)
        {
            Instance._PurchaseItemBundle(bundleId, paymentType, location, callback);
        }

        private void _PurchaseItemBundle(int bundleId, int paymentType, int location, Action<ItemBundlePurchaseResponse> callback)
        {
            Instance.StartCoroutine(ServiceGetString(callObjectCreator.ItemBundlePurchaseCall(new ItemBundlePurchaseRequest() { BundleID = bundleId, PaymentType = paymentType, Location = location }), x =>
            {
                //callback(responseCreator.CreateItemBundlePurchaseResponse(x));
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
            if (error!= null)
            {
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
