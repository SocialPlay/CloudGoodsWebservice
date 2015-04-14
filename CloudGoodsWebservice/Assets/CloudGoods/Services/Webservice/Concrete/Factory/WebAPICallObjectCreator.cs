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

        WWW GenerateWWWCall(string controller, params KeyValuePair<string, string>[] urlPrams)
        {
            string createdURL = "";
            foreach (KeyValuePair<string, string> urlA in urlPrams)
            {
                if (createdURL == "")
                    createdURL += "?";
                else
                    createdURL += "&";
                createdURL += urlA.Key + "=" + urlA.Value;
            }
            Dictionary<string, string> headers = CreateHeaders(createdURL);
            string urlString = string.Format("{0}api/CloudGoods/{1}{2}", CloudGoodsSettings.Url, controller, createdURL);
            return new WWW(urlString, null, headers);
        }


        WWW GenerateWWWCallWithoutUser(string controller, params KeyValuePair<string, string>[] urlPrams)
        {
            string createdURL = "";
            foreach (KeyValuePair<string, string> urlA in urlPrams)
            {
                if (createdURL == "")
                    createdURL += "?";
                else
                    createdURL += "&";
                createdURL += urlA.Key + "=" + urlA.Value;
            }
            Dictionary<string, string> headers = CreateHeaders(createdURL, false);
            string urlString = string.Format("{0}api/CloudGoods/{1}{2}", CloudGoodsSettings.Url, controller, createdURL);
            return new WWW(urlString, null, headers);
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

        public WWW CreateLoginCallObject(string userEmail, string password)
        {
            string loginUrl = string.Format("?appId={0}&email={1}&password={2}", CloudGoodsSettings.AppID, userEmail, password);

            Dictionary<string, string> headers = CreateHeaders(loginUrl, false);
            string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Login" + loginUrl);
            return new WWW(urlString, null, headers);
        }

        public WWW CreateLoginByPlatformCallObject(string userName, int platformId, string platformUserID)
        {
            string loginUrl = string.Format("?appId={0}&userName={1}&platformId={2}&platformUserId={3}", CloudGoodsSettings.AppID, userName, platformId, platformUserID);

            Dictionary<string, string> headers = CreateHeaders(loginUrl, false);
            string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/LoginByPlatform" + loginUrl);
            return new WWW(urlString, null, headers);
        }

        public WWW CreateRegisterUserCallObject(RegisterUserRequest request)
        {
            return GenerateWWWPost("RegisterUser", request, true);
        }

        public WWW CreateForgotPasswordCallObject(string userEmail)
        {
            return GenerateWWWCallWithoutUser("ForgotPassword", 
                GetParameter("appId", CloudGoodsSettings.AppID),
                GetParameter("userEmail", userEmail));
        }

        public WWW CreateResendVerificationEmailCallObject(string email)
        {
            return GenerateWWWCallWithoutUser(
                "ResendVerification",
                GetParameter("appId", CloudGoodsSettings.AppID),
                GetParameter("userEmail", email)
                );
        }

        public WWW CreateGetUserItemsCallObject(int location, string andTags = null, string orTags = null)
        {
            return GenerateWWWCallWithoutUser("UserItems"
                , GetParameter("location", location.ToString())
                , GetParameter("andTags", andTags)
                , GetParameter("orTags", orTags)
                );
        }

        public WWW CreateMoveItemsCallObject(MoveItemsRequest request)
        {
            return GenerateWWWPost("MoveItems", request);
        }

        public WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request)
        {
            return GenerateWWWPost("CreateItemVouchers", request);
        }

        public WWW CreateItemVoucherCall(int voucherId)
        {
            return GenerateWWWCall("ItemVoucher", new KeyValuePair<string, string>("voucherId", voucherId.ToString()));
        }

        public WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request)
        {
            return GenerateWWWPost("RedeemItemVouchers", request);
        }

        public WWW CreateUpdateItemByIdRequestCallObject(UpdateItemByIdRequest request)
        {
            return GenerateWWWPost("UpdateItemsById", request);
        }

        public WWW CreateUpdateItemByStackIdRequestCallObject(UpdateItemsByStackIdRequest request)
        {
            return GenerateWWWPost("UpdateItemsByStackId", request);
        }

        public WWW CreateItemBundlesCall(string andTags, string orTags)
        {
            return GenerateWWWCall("ItemBundles", new KeyValuePair<string, string>("andTags", andTags), new KeyValuePair<string, string>("orTags", orTags));
        }

        public WWW CreateCurrencyInfoCall()
        {
            return GenerateWWWCall("CurrencyInfo");
        }

        public WWW CreatePremiumCurrencyBalanceCall()
        {
            return GenerateWWWCall("PremiumCurrency");
        }

        public WWW CreateGetStoreItemsCall(string andTags, string orTags)
        {
            return GenerateWWWCall("StoreItems", new KeyValuePair<string, string>("andTags", andTags), new KeyValuePair<string, string>("orTags", orTags));
        }

        public WWW CreateStandardCurrencyBalanceCall(int accessLocation)
        {
            return GenerateWWWCall("StandardCurrency", new KeyValuePair<string, string>("accessLocation", accessLocation.ToString()));
        }

        public WWW CreateGetPremiumCurrencyBundlesCall(int PlatformId)
        {
            return GenerateWWWCall("PremiumCurrencyBundles", new KeyValuePair<string, string>("platformId", PlatformId.ToString()));
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


        public WWW CreateUserDataCall(string key)
        {
            return GenerateWWWCall("UserData", new KeyValuePair<string, string>("key", key));
        }


        public WWW CreateUserDataUpdateCall(string key, string value)
        {
            return GenerateWWWCall("UserDataUpdate", new KeyValuePair<string, string>("key", key), new KeyValuePair<string, string>("value", value));
        }

        public WWW CreateUserDataAllCall()
        {
            return GenerateWWWCall("UserDataAll");
        }

        public WWW CreateUserDataByKeyCall(string key)
        {
            return GenerateWWWCall("UserDataByKey", new KeyValuePair<string, string>("key", key));
        }


        public WWW CreateUserItemCall(int itemId, int location)
        {
            return GenerateWWWCall("UserItem", GetParameter("itemId", itemId.ToString()), GetParameter("location", location.ToString()));
        }


        public WWW CreateAppDataCall(string key)
        {
            return GenerateWWWCall("AppData", GetParameter("key", key));
        }

        public WWW CreateAppDataAllCall()
        {
            return GenerateWWWCall("AppDataAll");
        }

        public WWW CreateUpdateAppDataCall(string key, string value)
        {
            return GenerateWWWCall("AppDataUpdate", GetParameter("key", key), GetParameter("value", value));
        }
    }
}

