using UnityEngine;
using System.Collections;
using System;
using LitJson;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods;
using CloudGoods.Services.Webservice;

namespace CloudGoods.Services.Webservice
{
    public class LitJsonResponseCreator : ResponseCreator
    {

        #region UserManagement

        public CloudGoodsUser CreateLoginResponse(string responseData)
        {
            return JsonMapper.ToObject<CloudGoodsUser>(responseData);
        }

        public RegisteredUser CreateRegisteredUserResponse(string responseData)
        {
            return JsonMapper.ToObject<RegisteredUser>(responseData);
        }

        #endregion

        #region Item Management

        public List<InstancedItemInformation> CreateItemDataListResponse(string responseData)
        {
            return JsonMapper.ToObject<List<InstancedItemInformation>>(responseData);
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

        public InstancedItemInformation CreateInstancedItemInformationResponse(string responseData)
        {
            return JsonMapper.ToObject<InstancedItemInformation>(responseData);
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
                throw new Exception("Invalid Data received from webservice :" + data);
            }

            return true;
        }

        public WebserviceError IsWebserviceError(string data)
        {
            JsonData jsonData = JsonMapper.ToObject(data);


            if (JsonDataContainsKey(jsonData, "errorCode"))
            {
                return new WebserviceError(int.Parse(jsonData["errorCode"].ToString()), jsonData["message"].ToString());
            }

            return null;
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

        public StatusMessageResponse CreateStatusMessageResponse(string responseData)
        {
            return JsonMapper.ToObject<StatusMessageResponse>(responseData);
        }

        public ItemBundlePurchaseResponse CreateItemBundlePurchaseResponse(string responseData)
        {
            return JsonMapper.ToObject<ItemBundlePurchaseResponse>(responseData);
        }

        public List<PremiumCurrencyBundle> CreatePremiumCurrencyBundleResponse(string responseData)
        {
            return JsonMapper.ToObject<List<PremiumCurrencyBundle>>(responseData);
        }

        public ConsumePremiumResponce CreateConsumePremiumResponce(string responseData)
        {
            return JsonMapper.ToObject<ConsumePremiumResponce>(responseData);
        }


        public CloudData CreateCloudDataResponse(string responseData)
        {
            return JsonMapper.ToObject<CloudData>(responseData);
        }


        public List<OwnedCloudData> CreateUserDataByKeyResponse(string responseData)
        {
            return JsonMapper.ToObject<List<OwnedCloudData>>(responseData);
        }

        public List<CloudData> CreateCloudDataListResponse(string responseData)
        {
            return JsonMapper.ToObject<List<CloudData>>(responseData);
        }

     
    }

    public class WebserviceException : Exception
    {
        public WebserviceException(string errorCode, string message)
            : base("Error " + errorCode + ": " + message)
        {
        }
    }
}
