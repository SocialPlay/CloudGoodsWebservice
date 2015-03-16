using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudGoods.Models;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    List<ItemData> CreateItemDataListResponse(string responseData);

    UpdatedStacksResponse CreateUpdatedStacksResponse(string responseData);

    CreateItemVouchersResponse CreateCreateItemVoucherResponse(string responseData);

    RedeemItemVouchersResponse CreteRedeemItemVoucherResponse(string responseData);
 

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
