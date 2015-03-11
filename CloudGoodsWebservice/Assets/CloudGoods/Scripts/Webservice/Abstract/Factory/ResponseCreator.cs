using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudgoodsClasses;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    List<ItemData> CreateGetUserItemsResponse(string responseData);

    NewItemStack CreateMoveItemResponse(string responseData);

    GiveOwnerItemResponse CreateGiveOwnerItemResponse(string responseData);

    CreateItemVouchersResponse CreateCreateItemVoucherResponse(string responseData);

    ConsumeItemVouchersResponse CreteConsomeItemVoucherResponse(string responseData);
 

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
