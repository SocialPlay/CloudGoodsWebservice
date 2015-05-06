using UnityEngine;
using System.Collections;
using System;
using CloudGoods.Services;
using CloudGoods.Enums;

namespace CloudGoods.SDK.Models
{
    public class LoginByPlatformRequest : IRequestClass
    {
        public string AppId;
        public string UserName;
        public int PlatformId;
        public string PlatformUserId;
        public int DeviceType;

        public string ToHashable()
        {
            return AppId + UserName + PlatformId + PlatformUserId + DeviceType;
        }

        public LoginByPlatformRequest() { }

        public LoginByPlatformRequest(string userName, CloudGoodsPlatform platformId, string platformUserId, int deviceType =0)
        {
            UserName = userName;
            PlatformId = (int)platformId;
            PlatformUserId = platformUserId;
            AppId = CloudGoodsSettings.AppID;
            DeviceType = deviceType;
        }

        public LoginByPlatformRequest(string userName, int platformId, string platformUserId, int deviceType = 0)
        {
            UserName = userName;
            PlatformId = platformId;
            PlatformUserId = platformUserId;
            AppId = CloudGoodsSettings.AppID;
            DeviceType = deviceType;
        }
    }
}
