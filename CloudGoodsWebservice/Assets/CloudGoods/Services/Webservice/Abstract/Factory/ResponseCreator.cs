using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;

namespace CloudGoods.Services.Webservice
{

    public interface ResponseCreator
    {    

        bool IsValidData(string data);

        WebserviceError IsWebserviceError(string data);

        CloudGoodsUser CreateLoginResponse(string responseData);

        RegisteredUser CreateRegisteredUserResponse(string responseData);

        List<InstancedItemInformation> CreateItemDataListResponse(string responseData);

        UpdatedStacksResponse CreateUpdatedStacksResponse(string responseData);

        ItemVouchersResponse CreateItemVoucherResponse(string responseData);

        ItemBundlesResponse CreateItemBundlesResponse(string responseData);

        CurrencyInfoResponse CreateCurrencyInfoResponse(string responseData);

        CurrencyBalanceResponse CreateCurrencyBalanceResponse(string responseData);

        SimpleItemInfo CreateSimpleItemInfoResponse(string responseData);

        List<StoreItem> CreateGetStoreItemResponse(string responseData);

        ConsumePremiumResponce CreateConsumePremiumResponce(string responseData);

        ItemBundlePurchaseResponse CreateItemBundlePurchaseResponse(string responseData);

        StatusMessageResponse CreateStatusMessageResponse(string responseData);

        CloudData CreateCloudDataResponse(string responseData);

        List<OwnedCloudData> CreateUserDataByKeyResponse(string responseData);

        List<CloudData> CreateCloudDataListResponse(string responseData);

        InstancedItemInformation CreateInstancedItemInformationResponse(string responseData);

        List<PremiumCurrencyBundle> CreatePremiumCurrencyBundleResponse(string responseData);
    }

}
