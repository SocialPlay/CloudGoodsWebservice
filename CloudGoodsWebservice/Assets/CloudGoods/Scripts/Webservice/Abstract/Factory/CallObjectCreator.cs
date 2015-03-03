using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface CallObjectCreator  {

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGetUserItemsCallObject(string SessionID);

    WWW CreateGetServerTimeObject();

    Dictionary<string, string> CreateLoginCallHeader(string urlString);

    int GetTimestamp();

    Guid GenerateNonce();
}
