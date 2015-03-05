﻿using UnityEngine;
using System.Collections;
using System;
using LitJson;
using System.Collections.Generic;

public class LitJsonResponseCreator : ResponseCreator {

    #region UserManagement

    public CloudGoodsUser CreateLoginResponse(string responseData)
    {
        JsonData jsonData = JsonMapper.ToObject(responseData);

        return new CloudGoodsUser(jsonData["UserId"].ToString(), jsonData["Username"].ToString(), jsonData["Email"].ToString(), jsonData["SessionId"].ToString(), false);
    }

    #endregion

    #region Item Management

    public List<ItemData> CreateGetUserItemsResponse(string responseData)
    {

        JsonData jsonUserItems = JsonMapper.ToObject(responseData);

        List<ItemData> items = new List<ItemData>();

        for (int i = 0; i < jsonUserItems.Count; i++)
        {
            JsonData jsonItemData = jsonUserItems[i];

            ItemData itemData = new ItemData()
            {
                Amount = int.Parse(jsonItemData["amount"].ToString()),
                ClassId = int.Parse(jsonItemData["classId"].ToString()),
                CollectionId = int.Parse(jsonItemData["collectionId"].ToString()),
                Detail = jsonItemData["detail"].ToString(),
                Energy = int.Parse(jsonItemData["energy"].ToString()),
                Id = int.Parse(jsonItemData["Id"].ToString()),
                Location = int.Parse(jsonItemData["location"].ToString()),
                Name = jsonItemData["name"].ToString(),
                StackLocationId = jsonItemData["stackLocationId"].ToString()
            };

            for (int j = 0; j < jsonItemData["behaviours"].Count; j++)
            {
                itemData.behaviours.Add(new ItemData.Behaviours() { name = jsonItemData["behaviours"][j]["name"].ToString(), Id = int.Parse(jsonItemData["behaviours"][j]["Id"].ToString()) });
            }

            for (int j = 0; j < jsonItemData["tags"].Count; j++)
            {
                itemData.tags.Add(new ItemData.Tag() { name = jsonItemData["tags"][j]["name"].ToString(), Id = int.Parse(jsonItemData["tags"][j]["Id"].ToString()) });
            }

            items.Add(itemData);

        }

        return items;
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