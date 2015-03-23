﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudGoods.Models;

namespace CloudGoods.Webservice
{

    public interface ResponseCreator
    {    

        bool IsValidData(string data);

        WebserviceError IsWebserviceError(string data);

        CloudGoodsUser CreateLoginResponse(string responseData);

        List<InstancedItemInformation> CreateItemDataListResponse(string responseData);

        UpdatedStacksResponse CreateUpdatedStacksResponse(string responseData);

        ItemVouchersResponse CreateItemVoucherResponse(string responseData);

        ItemBundlesResponse CreateItemBundlesResponse(string responseData);

        CurrencyInfoResponse CreateCurrencyInfoResponse(string responseData);

        CurrencyBalanceResponse CreateCurrencyBalanceResponse(string responseData);

        SimpleItemInfo CreateSimpleItemInfoResponse(string responseData);

        List<StoreItem> CreateGetStoreItemResponse(string responseData);

    }

}
