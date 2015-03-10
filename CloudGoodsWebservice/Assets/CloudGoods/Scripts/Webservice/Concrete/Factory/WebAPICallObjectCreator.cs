﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

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

    #region User Management

    public WWW CreateLoginCallObject(string appID, string userEmail, string password)
    {
        string loginUrl = string.Format("?appId={0}&email={1}&password={2}", appID, userEmail, password);

        Dictionary<string, string> headers = CreateLoginCallHeader(loginUrl);
        string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Login" + loginUrl);
        return new WWW(urlString, null, headers);
    }

    #endregion

    #region Item Management

    public WWW CreateGetUserItemsCallObject(int location)
    {
        return GenerateWWWCall("UserItems", new KeyValuePair<string, string>("location", location.ToString()));
    }

    #endregion

    public WWW CreateMoveItemCallObject(string stackId, int amount, int location, string ownerType)
    {
        return GenerateWWWCall("MoveItem", new KeyValuePair<string, string>("StackId", stackId), new KeyValuePair<string, string>("amount", amount.ToString()), new KeyValuePair<string, string>("location", location.ToString()), new KeyValuePair<string, string>("ownerType", ownerType));
    }

    #region Server Utilities

    public WWW CreateGetServerTimeObject()
    {
        string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/Time");
        return new WWW(urlString);
    }

    #endregion

    #region Utilities

    public Dictionary<string, string> CreateLoginCallHeader(string urlString)
    {
        string timeStamp = GetTimestamp().ToString();

        Dictionary<string, string> headers = new Dictionary<string, string>();

        headers.Add("Hash", hashCreator.CreateHash(timeStamp, urlString));
        headers.Add("Timestamp", timeStamp);

        if (!string.IsNullOrEmpty(CloudGoods.Instance.SessionId))
        {
            headers.Add("SessionID", CloudGoods.Instance.SessionId);
            headers.Add("Nonce", GenerateNonce());
        }


        return headers;
    }

    public int GetTimestamp()
    {
        int timeStamp = DateTime.UtcNow.ConvertToUnixTimestamp() + CloudGoods.Instance.ServerTimeDifference;
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
        Dictionary<string, string> headers = CreateLoginCallHeader(createdURL);
        string urlString = string.Format("{0}api/CloudGoods/{1}{2}", CloudGoodsSettings.Url, controller, createdURL);
        Debug.Log(urlString);
        return new WWW(urlString, null, headers);
    }

    public WWW GenerateWWWPost(string controller, object dataObject)
    {
        Dictionary<string, string> headers = CreateLoginCallHeader("");
        headers.Add("Content-Type", "application/json");
        string urlString = string.Format("{0}api/CloudGoods/{1}", CloudGoodsSettings.Url, controller);
        byte[] body = Encoding.UTF8.GetBytes(LitJson.JsonMapper.ToJson(dataObject));
        return new WWW(urlString, body, headers);
    }

    #endregion

}

