using UnityEngine;
using System.Collections;
using System;

public interface ResponseCreator {

    event Action<WebserviceError> ResponseHasError;

    bool CheckForWebserviceError(object data);

    UserResponse CreateLoginResponse(string responseData);
}
