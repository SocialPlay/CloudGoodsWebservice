using UnityEngine;
using System.Collections;
using System;
using LitJson;

public class LitJsonResponseCreator : ResponseCreator {

    #region UserManagement

    public CloudGoodsUser CreateLoginResponse(string responseData)
    {
        if(!IsValidData(responseData)) return null;
        if(IsWebserviceError(responseData)) return null;

        JsonData jsonData = JsonMapper.ToObject(responseData);

        return new CloudGoodsUser(jsonData["UserId"].ToString(), jsonData["Username"].ToString(), jsonData["Email"].ToString(), jsonData["SessionId"].ToString(), false);
    }

    #endregion


    #region Utilities

    public bool IsValidData(string data)
    {
        try
        {
            JsonData jsonData = JsonMapper.ToObject(data);
        }
        catch
        {
            throw new Exception("Invalid Data received from webservice");
        }

        return true;
    }

    public bool IsWebserviceError(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);

        if (JsonDataContainsKey(jsonData, "errorCode"))
        {
            throw new WebserviceException(jsonData["errorCode"].ToString(), jsonData["message"].ToString());
        }

        return false;
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

    #endregion

}

public class WebserviceException : Exception
{
    public WebserviceException(string errorCode, string message) : base("Error " + errorCode + ": " + message)
    {
    }
}
