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
        public static void UserDataGet(string key, Action<CloudData> callback)
        {
            CallHandler.Instance.GetUserData(key, callback);
        }

        public static void UserDataUpdate(string key, string value, Action<CloudData> callback)
        {
            CallHandler.Instance.UserDataUpdate(key, value, callback);
        }

        public static void UserDataAll(Action<List<CloudData>> callback)
        {
            CallHandler.Instance.UserDataAll(callback);
        }

        public static void UserDataByKey(string key, Action<List<OwnedCloudData>> callback)
        {
            CallHandler.Instance.UserDataByKey(key, callback);
        }

        public static void AppData(string key, Action<CloudData> callback)
        {
            CallHandler.Instance.AppData(key, callback);
        }

        public static void AppDataAll(Action<List<CloudData>> callback)
        {
            CallHandler.Instance.AppDataAll(callback);
        }

        public static void UpdateAppData(string key, string value, Action<CloudData> callback)
        {
            CallHandler.Instance.UpdateAppData(key, value, callback);
        }
    }
}
