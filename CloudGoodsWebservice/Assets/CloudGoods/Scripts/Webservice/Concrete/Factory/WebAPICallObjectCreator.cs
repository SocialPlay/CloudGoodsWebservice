using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using CloudGoods;
using CloudGoods.Models;
using CloudGoodsUtilities;
using CloudGoods.Webservice;

namespace CloudGoods.Webservice
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



        public Dictionary<string, string> CreateHeaders(string dataString)
        {
            string timeStamp = GetTimestamp().ToString();

            Dictionary<string, string> headers = new Dictionary<string, string>();

            List<string> values = new List<string>();
            headers.Add("Timestamp", timeStamp);
            values.Add(timeStamp);
            if (!string.IsNullOrEmpty(CallHandler.SessionId))
            {
                headers.Add("SessionID", CallHandler.SessionId);
                values.Add(CallHandler.SessionId);
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

        public WWW GenerateWWWCall(string controller, params KeyValuePair<string, string>[] urlPrams)
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

        public WWW GenerateWWWPost(string controller, IRequestClass dataObject)
        {
            string objectString = LitJson.JsonMapper.ToJson(dataObject);
            Dictionary<string, string> headers = CreateHeaders(dataObject.ToHashable());
            headers.Add("Content-Type", "application/json");
            string urlString = string.Format("{0}api/CloudGoods/{1}", CloudGoodsSettings.Url, controller);
            byte[] body = Encoding.UTF8.GetBytes(objectString);
            return new WWW(urlString, body, headers);
        }

        #endregion

        public WWW CreateLoginCallObject(string userEmail, string password)
        {
            string loginUrl = string.Format("?appId={0}&email={1}&password={2}", CloudGoodsSettings.AppID, userEmail, password);

            Dictionary<string, string> headers = CreateHeaders(loginUrl);
            string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Login" + loginUrl);
            return new WWW(urlString, null, headers);
        }

        public WWW CreateLoginByPlatformCallObject(string userName, int platformId, string platformUserID)
        {
            string loginUrl = string.Format("?appId={0}&userName={1}&platformId={2}&platformUserId={3}", CloudGoodsSettings.AppID, userName, platformId, platformUserID);

            Dictionary<string, string> headers = CreateHeaders(loginUrl);
            string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/LoginByPlatform" + loginUrl);
            return new WWW(urlString, null, headers);
        }

        public WWW CreateRegisterUserCallObject(RegisterUserRequest request)
        {
            return GenerateWWWPost("RegisterUser", request);
        }

        public WWW CreateForgotPasswordCallObject(string userEmail)
        {
            return GenerateWWWCall("ForgotPassword", new KeyValuePair<string, string>("appId", CloudGoodsSettings.AppID),
                new KeyValuePair<string, string>("userEmail", userEmail));
        }

        public WWW CreateResendVerificationEmailCallObject(string email)
        {
            return GenerateWWWCall("ResendVerification", new KeyValuePair<string, string>("appId", CloudGoodsSettings.AppID),
                new KeyValuePair<string, string>("userEmail", email));
        }

        public WWW CreateGetUserItemsCallObject(int location, string ownerType = "User", string ownerId = "Default")
        {
            return GenerateWWWCall("UserItems", new KeyValuePair<string, string>("location", location.ToString()),
                new KeyValuePair<string, string>("location", ownerType),
                    new KeyValuePair<string, string>("location", ownerId));
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
    }
}

