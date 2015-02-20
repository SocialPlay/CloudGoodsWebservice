using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WebAPICallObjectCreator : CallObjectCreator {

    public HashCreator hashCreator = new StandardHashCreator();
    string url = "http://localhost:11240/";

    #region UserManagement

    public WWW CreateLoginCallObject(string appID, string userEmail, string password)
    {
        string loginUrl = string.Format("Login?appId={0}&email={1}&password={2}", appID, userEmail, password);

        Dictionary<string, string> headers = CreateCallHeader( loginUrl);

        string urlString = string.Format(url + "api/CloudGoods/" + loginUrl);
        return new WWW(urlString, null, headers);
    }

    #endregion


    public Dictionary<string, string> CreateCallHeader(string urlString)
    {
        string timeStamp = GetTimestamp().ToString();

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Hash", hashCreator.CreateHash(urlString, timeStamp));
        headers.Add("Timestamp", GetTimestamp().ToString());
        headers.Add("Authorization", "Here");

        return headers;
    }


    public int GetTimestamp()
    {
        DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan currentTs = DateTime.UtcNow - epochStart;
        return (int)(currentTs.TotalSeconds);
    }

    public Guid GenerateNonce()
    {
        return Guid.NewGuid();
    }

}
