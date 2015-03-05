using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

    public WWW CreateGetUserItemsCallObject(string SessionID)
    {      

       return GrenerateWWWCall("UserItems",new URLValue("location",0));
    }

    #endregion

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
        Debug.Log("Timestamp: " + timeStamp);
        return timeStamp;
    }

    public string GenerateNonce()
    {
        return Guid.NewGuid().ToString();
    }

    public WWW GrenerateWWWCall(string controller, params URLValue[] urlPrams)
    {
        string createdURL = "";
        foreach (URLValue urlA in urlPrams)
        {
            if (createdURL == "")
                createdURL += "?";
            else
                createdURL += "&";
            createdURL += urlA.Key + "=" + urlA.Value;
        }
        Dictionary<string, string> headers = CreateLoginCallHeader(createdURL);
        string urlString = string.Format(CloudGoodsSettings.Url + "api/CloudGoods/" + controller + createdURL);
        return new WWW(urlString, null, headers);
    }

    #endregion

}
