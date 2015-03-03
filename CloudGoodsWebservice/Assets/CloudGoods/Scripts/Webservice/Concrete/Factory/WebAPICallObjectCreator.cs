using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WebAPICallObjectCreator : CallObjectCreator {

    public HashCreator hashCreator = new StandardHashCreator();
    string url = "http://localhost:2842/";
    //string url = "http://192.168.0.197/WebService2/";

    #region User Management

    public WWW CreateLoginCallObject(string appID, string userEmail, string password)
    {
        string loginUrl = string.Format("?appId={0}&email={1}&password={2}", appID, userEmail, password);

        Dictionary<string, string> headers = CreateLoginCallHeader( loginUrl);

        string urlString = string.Format(url + "api/CloudGoods/Login" + loginUrl);
        return new WWW(urlString, null, headers);
    }

    #endregion

    #region Item Management

    public WWW CreateGetUserItemsCallObject(string SessionID)
    {
        string loginUrl = string.Format("?location={0}&andTags={1}&orTags={2}", 0, "null", "null");

        Debug.Log("LoginURL: " + loginUrl);

        Dictionary<string, string> headers = CreateLoginCallHeader(loginUrl);

        string urlString = string.Format(url + "api/CloudGoods/UserItems" + loginUrl);
        return new WWW(urlString, null, headers);
    }

    #endregion

    #region Server Utilities

    public WWW CreateGetServerTimeObject()
    {
        string urlString = string.Format(url + "api/CloudGoods/Time");
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
        headers.Add("Authorization", "Here");

        if (!string.IsNullOrEmpty(CloudGoods.Instance.SessionId))
        {
            headers.Add("SessionID", CloudGoods.Instance.SessionId);
            headers.Add("Nonce", Guid.NewGuid().ToString());
        }
            

        return headers;
    }

    public int GetTimestamp()
    {
        int timeStamp = DateTime.UtcNow.ConvertToUnixTimestamp() + CloudGoods.Instance.ServerTimeDifference;
        Debug.Log("Timestamp: " + timeStamp);
        return timeStamp;
    }

    public Guid GenerateNonce()
    {
        return Guid.NewGuid();
    }

    #endregion

}
