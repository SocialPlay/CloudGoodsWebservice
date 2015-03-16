using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudGoods.Models;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    List<ItemData> CreateItemDataListResponse(string responseData);

    UpdatedStacksResponse CreateUpdatedStacksResponse(string responseData);

    ItemVouchersResponse CreateItemVoucherResponse(string responseData);
 

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
