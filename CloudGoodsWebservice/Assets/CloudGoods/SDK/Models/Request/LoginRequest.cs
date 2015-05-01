using UnityEngine;
using System.Collections;
using CloudGoods.Services;
using System;

namespace CloudGoods.SDK.Models
{
    public class LoginRequest : IRequestClass
    {
        public string AppId;
        public string UserEmail;
        public string Password;
        public int DeviceType;

        public string ToHashable()
        {
            return AppId + UserEmail + Password + DeviceType;
        }

        public LoginRequest(string userEmail, string password, int deviceType = 0)
        {
            AppId = CloudGoodsSettings.AppID;
            UserEmail = userEmail;
            Password = password;
            DeviceType = deviceType;
        }
    }
}
