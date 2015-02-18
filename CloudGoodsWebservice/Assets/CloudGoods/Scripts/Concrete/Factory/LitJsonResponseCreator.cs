using UnityEngine;
using System.Collections;
using System;
using LitJson;

public class LitJsonResponseCreator : ResponseCreator {

    public event Action<WebserviceError> ResponseHasError;

    public bool CheckForWebserviceError(object data)
    {
        JsonData jsonData = (JsonData)data;

        if(JsonDataContainsKey(jsonData, "Error"))
        {
            if (ResponseHasError != null)
            {
                WebserviceError webServiceError = CreateWebserviceErrorObject(jsonData);
                ResponseHasError(webServiceError);
            }
            return true;
        }

        return false;
    }

    public UserResponse CreateLoginResponse(string responseData)
    {
        JsonData jsonData = JsonMapper.ToObject(responseData);

        if (!CheckForWebserviceError(responseData))
        {
            Debug.Log("No error");
            return new UserResponse(0, "", new CloudGoodsUser("", "", ""));
        }
        else
            return null;
    }

    public WebserviceError CreateWebserviceErrorObject(JsonData jsonErrorString)
    {
        WebserviceError webServiceError = new WebserviceError(int.Parse(jsonErrorString["ErrorCode"].ToString()), jsonErrorString["Message"].ToString());
        return webServiceError;
    }

    public bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }
}
