using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    List<ItemData> CreateGetUserItemsResponse(string responseData);

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
