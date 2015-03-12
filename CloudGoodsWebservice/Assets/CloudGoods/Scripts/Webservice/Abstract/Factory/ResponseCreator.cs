using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CloudgoodsClasses;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    List<ItemData> CreateItemDataListResponse(string responseData);

    MoveItemResponse CreateMoveItemResponse(string responseData);

    GiveOwnerItemResponse CreateGiveOwnerItemResponse(string responseData);

    CreateItemVouchersResponse CreateCreateItemVoucherResponse(string responseData);

    RedeemItemVouchersResponse CreteRedeemItemVoucherResponse(string responseData);
 

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
