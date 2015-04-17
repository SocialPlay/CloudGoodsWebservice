using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.Services.WebCommunication;


namespace CloudGoods.Services
{
    public class CloudDataServices
    {
        public static void UserDataGet(UserDataRequest request, Action<CloudData> callback)
        {
            CallHandler.Instance.GetUserData(request, callback);
        }

        public static void UserDataUpdate(UserDataUpdateRequest request, Action<CloudData> callback)
        {
            CallHandler.Instance.UserDataUpdate(request, callback);
        }

        public static void UserDataAll(Action<List<CloudData>> callback)
        {
            CallHandler.Instance.UserDataAll(callback);
        }

        public static void UserDataByKey(UserDataByKeyRequest request, Action<List<OwnedCloudData>> callback)
        {
            CallHandler.Instance.UserDataByKey(request, callback);
        }

        public static void AppData(AppDataRequest request, Action<CloudData> callback)
        {
            CallHandler.Instance.AppData(request, callback);
        }

        public static void AppDataAll(Action<List<CloudData>> callback)
        {
            CallHandler.Instance.AppDataAll(callback);
        }

        public static void UpdateAppData(AppDataUpdateRequest request , Action<CloudData> callback)
        {
            CallHandler.Instance.UpdateAppData(request, callback);
        }
    }
}
