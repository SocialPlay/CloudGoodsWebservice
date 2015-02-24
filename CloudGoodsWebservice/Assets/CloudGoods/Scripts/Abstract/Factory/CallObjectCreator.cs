using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface CallObjectCreator  {

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGetServerTimeObject();

    Dictionary<string, string> CreateCallHeader(string urlString);

    int GetTimestamp();

    Guid GenerateNonce();
}
