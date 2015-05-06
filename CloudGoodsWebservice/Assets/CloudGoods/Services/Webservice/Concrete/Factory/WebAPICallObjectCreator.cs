using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using CloudGoods.Services;
using CloudGoods.SDK.Models;
using CloudGoodsUtilities;
using CloudGoods.Services.Webservice;
using CloudGoods.Services.WebCommunication;

namespace CloudGoods.Services.Webservice
{
    public class WebAPICallObjectCreator : CallObjectCreator
    {
        public HashCreator hashCreator = new StandardHashCreator();

        public class URLValue
        {
            public string Key;
            public string Value;

            public URLValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public URLValue(string key, int value)
            {
                Key = key;
                Value = value.ToString();
            }
        }


        #region Server Utilities

        public WWW CreateGetServerTimeObject()
        {
            string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Time");

            Dictionary<string, string> headers = new Dictionary<string, string>();
            return new WWW(urlString);
        }

        #endregion

        #region Utilities



        public Dictionary<string, string> CreateHeaders(string dataString, bool isFull = true)
        {
            string timeStamp = GetTimestamp().ToString();

            Dictionary<string, string> headers = new Dictionary<string, string>();

            List<string> values = new List<string>();
            headers.Add("Timestamp", timeStamp);
            values.Add(timeStamp);
            if (isFull)
            {
                headers.Add("SessionID", AccountServices.ActiveUser.SessionId);
                values.Add(AccountServices.ActiveUser.SessionId);
                string nonce = GenerateNonce();
                headers.Add("Nonce", nonce);
                values.Add(nonce);
            }
            values.Add(dataString);
            headers.Add("Hash", hashCreator.CreateHash(values.ToArray()));
            return headers;
        }

        public Dictionary<string, string> CreatePostHeaders(IRequestClass requestObject)
        {
            return CreateHeaders(requestObject.ToHashable());
        }

        public int GetTimestamp()
        {
            int timeStamp = DateTime.UtcNow.ConvertToUnixTimestamp() + CallHandler.ServerTimeDifference;
            return timeStamp;
        }

        public string GenerateNonce()
        {
            return Guid.NewGuid().ToString();
        }

        private KeyValuePair<string, string> GetParameter(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        WWW GenerateWWWPost(string controller, IRequestClass dataObject, bool fullHeaders = true)
        {
            string objectString = LitJson.JsonMapper.ToJson(dataObject);
            Dictionary<string, string> headers = CreateHeaders(dataObject.ToHashable(), fullHeaders);
            headers.Add("Content-Type", "application/json");
            string urlString = string.Format("{0}api/CloudGoods/{1}", CloudGoodsSettings.Url, controller);
            byte[] body = Encoding.UTF8.GetBytes(objectString);
            return new WWW(urlString, body, headers);
        }

        #endregion

        #region Account Management
        public WWW CreateLoginCallObject(LoginRequest request)
        {

            return GenerateWWWPost("Login", request, false);
        }

        public WWW CreateLoginByPlatformCallObject(LoginByPlatformRequest request)
        {
            return GenerateWWWPost("LoginByPlatform", request, false);
        }

        public WWW CreateRegisterUserCallObject(RegisterUserRequest request)
        {
            return GenerateWWWPost("RegisterUser", request, false);
        }

        public WWW CreateForgotPasswordCallObject(ForgotPasswordRequest request)
        {
            return GenerateWWWPost("ForgotPassword", request, false);
        }

        public WWW CreateResendVerificationEmailCallObject(ResendVerificationRequest request)
        {
            return GenerateWWWPost("ResendVerification", request, false);
        }
        #endregion

        #region Item Manipulation

        public WWW CreateUserItemsCallObject(UserItemsRequest request)
        {
            return GenerateWWWPost("UserItems", request);
        }

        public WWW CreateInstanceItemsRequest(InstanceItemsRequest request)
        {
            return GenerateWWWPost("InstanceItems", request);
        }

        public WWW CreateSessionItemsCallObject(SessionItemsRequest request)
        {
            return GenerateWWWPost("SessionItems", request);
        }

        public WWW CreateUserItemCall(OwnerItemRequest request)
        {
            return GenerateWWWPost("UserItem", request);
        }

        public WWW CreateMoveItemsCallObject(MoveItemsRequest request)
        {
            return GenerateWWWPost("MoveItems", request);
        }

        public WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request)
        {
            return GenerateWWWPost("CreateItemVouchers", request);
        }

        public WWW CreateItemVoucherCall(ItemVoucherRequest request)
        {
            return GenerateWWWPost("ItemVoucher", request);
        }

        public WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request)
        {
            return GenerateWWWPost("RedeemItemVouchers", request);
        }

        public WWW CreateUpdateItemByIdRequestCallObject(UpdateItemsByIdsRequest request)
        {
            return GenerateWWWPost("UpdateItemsById", request);
        }

        public WWW CreateUpdateItemByStackIdRequestCallObject(UpdateItemsByStackIdRequest request)
        {
            return GenerateWWWPost("UpdateItemsByStackId", request);
        }
        #endregion

        public WWW CreateItemBundlesCall(ItemBundlesRequest request)
        {
            return GenerateWWWPost("ItemBundles", request);
        }

        public WWW CreateCurrencyInfoCall(CurrencyInfoRequest request)
        {
            return GenerateWWWPost("CurrencyInfo", request);
        }

        public WWW CreatePremiumCurrencyBalanceCall(PremiumCurrencyBalanceRequest request)
        {
            return GenerateWWWPost("PremiumCurrency", request);
        }

        public WWW CreateStoreItemsCall(StoreItemsRequest request)
        {
            return GenerateWWWPost("StoreItems", request);
        }

        public WWW CreateStandardCurrencyBalanceCall(StandardCurrencyBalanceRequest request)
        {
            return GenerateWWWPost("StandardCurrency", request);
        }

        public WWW CreatePremiumCurrencyBundlesCall(PremiumBundlesRequest request)
        {
            return GenerateWWWPost("PremiumCurrencyBundles", request);
        }

        public WWW ItemBundlePurchaseCall(ItemBundlePurchaseRequest request)
        {
            return GenerateWWWPost("ItemBundlePurchase", request);

        }

        public WWW CreateConsumePremiumCall(ConsumePremiumRequest request)
        {
            return GenerateWWWPost("ConsumePremium", request);
        }

        public WWW CreatePurchaseItemCall(PurchaseItemRequest request)
        {
            return GenerateWWWPost("PurchaseItem", request);
        }


        public WWW CreateUserDataCall(UserDataRequest request)
        {
            return GenerateWWWPost("UserData", request);
        }


        public WWW CreateUserDataUpdateCall(UserDataUpdateRequest request)
        {
            return GenerateWWWPost("UserDataUpdate", request);
        }

        public WWW CreateUserDataAllCall(UserDataAllRequest request)
        {
            return GenerateWWWPost("UserDataAll", request);
        }

        public WWW CreateUserDataByKeyCall(UserDataByKeyRequest request)
        {
            return GenerateWWWPost("UserDataByKey", request);
        }

        public WWW CreateAppDataCall(AppDataRequest request)
        {
            return GenerateWWWPost("AppData", request);
        }

        public WWW CreateAppDataAllCall(AppDataAllRequest request)
        {
            return GenerateWWWPost("AppDataAll", request);
        }

        public WWW CreateAppDataUpdateCall(AppDataUpdateRequest request)
        {
            return GenerateWWWPost("AppDataUpdate", request);
        }

        public WWW CreateSteamPremiumPurchaseCall(SteamPurchaseRequest request)
        {
            return GenerateWWWPost("SteamPremiumCurrencyPurchase", request);
        }

        public WWW CreateSteamOrderConfirmationCall(SteamOrderConfirmationRequest request)
        {
            return GenerateWWWPost("SteamOrderConfirmation", request);
        }


        public WWW CreatePremiumCurrencyBundlePurchaseCall(BundlePurchaseRequest request)
        {
            return GenerateWWWPost("PremiumBundlePurchase", request);
        }
    }
}

