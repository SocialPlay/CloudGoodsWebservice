using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WebAPICallObjectCreator : CallObjectCreator {

    public HashCreator hashCreator = new StandardHashCreator();
    //string url = "http://localhost:11240/";
    string url = "http://192.168.0.197/WebService2/";

    #region UserManagement

    public WWW CreateLoginCallObject(string appID, string userEmail, string password)
    {
        string loginUrl = string.Format("?appId={0}&email={1}&password={2}", appID, userEmail, password);

        Dictionary<string, string> headers = CreateCallHeader( loginUrl);

        string urlString = string.Format(url + "api/CloudGoods/Login" + loginUrl);
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

    public Dictionary<string, string> CreateCallHeader(string urlString)
    {
        string timeStamp = GetTimestamp().ToString();

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Hash", hashCreator.CreateHash(timeStamp, urlString));
        headers.Add("Timestamp", timeStamp);
        headers.Add("Authorization", "Here");

        return headers;
    }

    public int GetTimestamp()
    {
        int timeStamp = DateTime.UtcNow.ConvertToUnixTimestamp() + CloudGoods.Instance().ServerTimeDifference;
        Debug.Log("Timestamp: " + timeStamp);
        return timeStamp;
    }

    public Guid GenerateNonce()
    {
        return Guid.NewGuid();
    }

    #endregion

}
