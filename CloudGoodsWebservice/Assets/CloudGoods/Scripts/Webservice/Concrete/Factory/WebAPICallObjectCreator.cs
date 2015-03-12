﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using CloudgoodsClasses;
using CloudGoodsUtilities;

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

    public WWW CreateLoginCallObject(string appID, string userEmail, string password)
    {
        string loginUrl = string.Format("?appId={0}&email={1}&password={2}", appID, userEmail, password);

        Dictionary<string, string> headers = CreateHeaders(loginUrl);
        string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Login" + loginUrl);
        return new WWW(urlString, null, headers);
    }

    public WWW CreateGetUserItemsCallObject(int location, string ownerType = "User", string ownerId = "Default")
    {
        return GenerateWWWCall("UserItems", new KeyValuePair<string, string>("location", location.ToString()));
    }

    public WWW CreateMoveItemsCallObject(MoveItemsRequest request)
    {
        return GenerateWWWPost("MoveItems", request);
   }

    public WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request)
    {
        return GenerateWWWPost("CreateItemVouchers", request);
    }

    public WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request)
    {
        return GenerateWWWPost("ConsumeItemVouchers", request);
    }

    public WWW CreateGiveOwnerItemsCallObject(GiveOwnerItemRequest request)
    {
        return GenerateWWWPost("GiveOwnerItems", request);
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
        if (!string.IsNullOrEmpty(CloudGoods.SessionId))
        {
            headers.Add("SessionID", CloudGoods.SessionId);
            values.Add(CloudGoods.SessionId);
            string nonce = GenerateNonce();
            headers.Add("Nonce", nonce);
            values.Add(nonce);
        }
        values.Add(dataString);
        headers.Add("Hash", hashCreator.CreateHash(values.ToArray()));
        return headers;
    }

    public Dictionary<string, string> CreatePostHeaders(RequestClass requestObject)
    {
        return CreateHeaders(requestObject.ToHashable());
    }

    public int GetTimestamp()
    {
        int timeStamp = DateTime.UtcNow.ConvertToUnixTimestamp() + CloudGoods.ServerTimeDifference;
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

    public WWW GenerateWWWPost(string controller, RequestClass dataObject)
    {
        string objectString = LitJson.JsonMapper.ToJson(dataObject);
        Dictionary<string, string> headers = CreateHeaders(dataObject.ToHashable());
        headers.Add("Content-Type", "application/json");
        string urlString = string.Format("{0}api/CloudGoods/{1}", CloudGoodsSettings.Url, controller);
        byte[] body = Encoding.UTF8.GetBytes(objectString);
        return new WWW(urlString, body, headers);
    }

    #endregion






 
}

