using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudgoodsClasses;

public interface CallObjectCreator
{

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGiveOwnerItemsCallObject(GiveOwnerItemRequest request);

    WWW CreateGetOwnerItemsCallObject(int location, string ownerType = "User", string ownerId = "Default");

    WWW CreateMoveItemCallObject(string stackId, int amount, int location, string ownerType = "User", string ownerId="Default");

    WWW CreateGetServerTimeObject();

    WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request);

    WWW CreateConsumeItemVouchersCall(ConsumeItemVouchersRequest request);

    Dictionary<string, string> CreateHeaders(string urlString);

    Dictionary<string, string> CreatePostHeaders(RequestClass requestObject);

    int GetTimestamp();

    string GenerateNonce();
}
