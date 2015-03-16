using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CallHandler.Models;

public interface CallObjectCreator
{

    WWW CreateLoginCallObject(string appID, string userEmail, string password);

    WWW CreateGetUserItemsCallObject(int location, string ownerType = "User", string ownerId = "Default");

    WWW CreateMoveItemsCallObject(MoveItemsRequest request);

    WWW CreateGetServerTimeObject();

    WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request);

    WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request);

    WWW CreateUpdateItemByIdRequestCallObject(UpdateItemByIdRequest request);

    WWW CreateUpdateItemByStackIdRequestCallObject(UpdateItemsByStackIdRequest request);

    Dictionary<string, string> CreateHeaders(string urlString);

    Dictionary<string, string> CreatePostHeaders(RequestClass requestObject);

    int GetTimestamp();

    string GenerateNonce();
}
