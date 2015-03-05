using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface CallObjectCreator  {

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGetUserItemsCallObject(int location);

    WWW CreateMoveItemCallObject(string stackId, int amount, int location, string ownerType);

    WWW CreateGetServerTimeObject();

    Dictionary<string, string> CreateLoginCallHeader(string urlString);

    int GetTimestamp();

    string GenerateNonce();
}
