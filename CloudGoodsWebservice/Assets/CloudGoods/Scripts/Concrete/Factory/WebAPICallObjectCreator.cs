using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WebAPICallObjectCreator : CallObjectCreator {

    public HashCreator hashCreator = new StandardHashCreator();

    public WWW CreateLoginCallObject(string appID, string userName, string userEmail, string password)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Hash", hashCreator.CreateHash(appID, userName, userEmail, password));

        string urlString = string.Format("http://192.168.0.197/WebService2/api/CloudGoods/Login?appId={0}&email={1}&password={2}", appID, userEmail, password);
        return new WWW(urlString, null, headers); 
    }
}
