using UnityEngine;
using System.Collections;
using System;

public interface ResponseCreator {

    CloudGoodsUser CreateLoginResponse(string responseData);

    bool IsValidData(string data);

    bool IsWebserviceError(string data);
}
