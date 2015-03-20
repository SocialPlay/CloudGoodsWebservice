using UnityEngine;
using System.Collections;
using System;
using LitJson;
using System.Collections.Generic;
using CloudGoods.Models;
using CloudGoods;
using CloudGoods.Webservice;

namespace CloudGoods.Webservice
{
    public class LitJsonResponseCreator : ResponseCreator
    {

        #region UserManagement

        public CloudGoodsUser CreateLoginResponse(string responseData)
        {
            return JsonMapper.ToObject<CloudGoodsUser>(responseData);
        }

        #endregion

        #region Item Management

        public List<ItemData> CreateItemDataListResponse(string responseData)
        {
            return JsonMapper.ToObject<List<ItemData>>(responseData);
        }

        public UpdatedStacksResponse CreateUpdatedStacksResponse(string responseData)
        {
            return JsonMapper.ToObject<UpdatedStacksResponse>(responseData);
        }

        public UpdatedStacksResponse CreateGiveOwnerItemResponse(string responseData)
        {
            return JsonMapper.ToObject<UpdatedStacksResponse>(responseData);
        }

        public ItemVouchersResponse CreateItemVoucherResponse(string responseData)
        {
            return JsonMapper.ToObject<ItemVouchersResponse>(responseData);
        }



        #endregion

        #region Store
        public ItemBundlesResponse CreateItemBundlesResponse(string responseData)
        {
            return JsonMapper.ToObject<ItemBundlesResponse>(responseData);
        }

        public CurrencyInfoResponse CreateCurrencyInfoResponse(string responseData)
        {
            return JsonMapper.ToObject<CurrencyInfoResponse>(responseData);
        }

        public CurrencyBalanceResponse CreateCurrencyBalanceResponse(string responseData)
        {
            return JsonMapper.ToObject<CurrencyBalanceResponse>(responseData);
        }

        public List<StoreItem> CreateGetStoreItemResponse(string responseData)
        {
            JsonData data = JsonMapper.ToObject(responseData);
            return JsonMapper.ToObject<List<StoreItem>>(data["StoreItems"].ToJson());
        }

        public SimpleItemInfo CreateSimpleItemInfoResponse(string responseData)
        {
            return JsonMapper.ToObject<SimpleItemInfo>(responseData);
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
        public WebserviceException(string errorCode, string message)
            : base("Error " + errorCode + ": " + message)
        {
        }
    }
}
