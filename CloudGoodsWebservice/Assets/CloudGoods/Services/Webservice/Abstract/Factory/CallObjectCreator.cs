using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CloudGoods.SDK.Models;

namespace CloudGoods.Services.Webservice
{

    public interface CallObjectCreator
    {
        int GetTimestamp();

        string GenerateNonce();

        Dictionary<string, string> CreateHeaders(string urlString, bool isFull = true);

        Dictionary<string, string> CreatePostHeaders(IRequestClass requestObject);

        WWW CreateLoginCallObject(LoginRequest request);

        WWW CreateRegisterUserCallObject(RegisterUserRequest request);

        WWW CreateForgotPasswordCallObject(ForgotPasswordRequest request);

        WWW CreateResendVerificationEmailCallObject(ResendVerificationRequest request);

        WWW CreateLoginByPlatformCallObject( LoginByPlatformRequest request);

        WWW CreateUserItemsCallObject(UserItemsRequest request);

        WWW CreateInstanceItemsRequest(InstanceItemsRequest request);

        WWW CreateSessionItemsCallObject(SessionItemsRequest request);

        WWW CreateUserItemCall(OwnerItemRequest request);

        WWW CreateMoveItemsCallObject(MoveItemsRequest request);

        WWW CreateGetServerTimeObject();/// only get call

        WWW CreateCreateItemVouchersCall(CreateItemVouchersRequest request);

        WWW CreateItemVoucherCall(ItemVoucherRequest request);

        WWW CreateRedeemItemVouchersCall(RedeemItemVouchersRequest request);

        WWW CreateUpdateItemByIdRequestCallObject(UpdateItemsByIdsRequest request);

        WWW CreateUpdateItemByStackIdRequestCallObject(UpdateItemsByStackIdRequest request);

        WWW CreateItemBundlesCall(ItemBundlesRequest request);

        WWW CreateCurrencyInfoCall(CurrencyInfoRequest request);

        WWW CreatePremiumCurrencyBalanceCall(PremiumCurrencyBalanceRequest request);

        WWW CreatePremiumCurrencyBundlesCall(PremiumBundlesRequest request);

        WWW CreatePremiumCurrencyBundlePurchaseCall(BundlePurchaseRequest request);

        WWW CreateStoreItemsCall(StoreItemsRequest request);

        WWW CreateStandardCurrencyBalanceCall(StandardCurrencyBalanceRequest request);

        WWW ItemBundlePurchaseCall(ItemBundlePurchaseRequest request);

        WWW CreateConsumePremiumCall(ConsumePremiumRequest request);

        WWW CreatePurchaseItemCall(PurchaseItemRequest request);

        WWW CreateUserDataCall(UserDataRequest request);

        WWW CreateUserDataUpdateCall(UserDataUpdateRequest request);

        WWW CreateUserDataAllCall(UserDataAllRequest request);

        WWW CreateUserDataByKeyCall(UserDataByKeyRequest request);

        WWW CreateAppDataCall(AppDataRequest request);

        WWW CreateAppDataAllCall(AppDataAllRequest request);

        WWW CreateAppDataUpdateCall(AppDataUpdateRequest request);

        WWW CreateSteamPremiumPurchaseCall(SteamPurchaseRequest request);

        WWW CreateSteamOrderConfirmationCall(SteamOrderConfirmationRequest request);
    }
}
