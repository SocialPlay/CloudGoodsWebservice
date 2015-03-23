﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.Models;

namespace CloudGoods.Webservice
{

    public interface CallObjectCreator
    {

        WWW CreateLoginCallObject(string appID, string userEmail, string password);

        WWW CreateGetUserItemsCallObject(int location, string ownerType = "User", string ownerId = "Default");

        WWW CreateMoveItemsCallObject(MoveItemsRequest request);

        WWW CreateGetServerTimeObject();

        WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request);

        WWW CreateItemVoucherCall(int voucherId);

        WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request);

        WWW CreateUpdateItemByIdRequestCallObject(UpdateItemByIdRequest request);

        WWW CreateUpdateItemByStackIdRequestCallObject(UpdateItemsByStackIdRequest request);

        WWW CreateItemBundlesCall(string andTags, string orTags);

        WWW CreateCurrencyInfoCall();

        WWW CreatePremiumCurrencyBalanceCall();

        WWW CreateGetStoreItemsCall(string andTags, string orTags);

        WWW CreateStandardCurrencyBalanceCall(int accessLocation);

        Dictionary<string, string> CreateHeaders(string urlString);

        Dictionary<string, string> CreatePostHeaders(IRequestClass requestObject);

        int GetTimestamp();

        string GenerateNonce();

        WWW ItemBundlePurchaseCall(ItemBundlePurchaseRequest request);
    }
}
