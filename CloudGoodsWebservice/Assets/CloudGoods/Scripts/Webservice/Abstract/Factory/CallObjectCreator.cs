using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudgoodsClasses;

public interface CallObjectCreator
{

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGiveOwnerItemsCallObject(GiveOwnerItemRequest request);

    WWW CreateGetUserItemsCallObject(int location, string ownerType = "User", string ownerId = "Default");

    WWW CreateMoveItemsCallObject(MoveItemsRequest request);

    WWW CreateGetServerTimeObject();

    WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request);

    WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request);

    Dictionary<string, string> CreateHeaders(string urlString);

    Dictionary<string, string> CreatePostHeaders(RequestClass requestObject);

    int GetTimestamp();

    string GenerateNonce();
}
