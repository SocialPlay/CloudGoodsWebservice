﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.Models;

namespace CloudGoods.Webservice
{

    public interface CallObjectCreator
    {
        WWW CreateLoginCallObject(string userEmail, string password);

        WWW CreateRegisterUserCallObject(RegisterUserRequest request);

        WWW CreateForgotPasswordCallObject(string userEmail);

        WWW CreateResendVerificationEmailCallObject(string email);

        WWW CreateLoginByPlatformCallObject( string userName, int platformId, string platformUserID);

        WWW CreateGetUserItemsCallObject(int location, string andTags = null, string orTags = null);

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

        WWW CreateConsumePremiumCall(ConsumePremiumRequest request);

        WWW CreatePurchaseItemCall(PurchaseItemRequest request);

        WWW CreateUserDataCall(string key);

        WWW CreateUserDataUpdateCall(string key, string value);

        WWW CreateUserDataAllCall();

        WWW CreateUserDataByKeyCall(string key);

        WWW CreateUserItemCall(int lookupItemId, int location);

        WWW CreateAppDataCall(string key);

        WWW CreateAppDataAllCall();

        WWW CreateUpdateAppDataCall(string key, string value);
    }
}
