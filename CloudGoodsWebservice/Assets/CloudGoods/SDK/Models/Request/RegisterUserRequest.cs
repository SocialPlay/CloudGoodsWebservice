using UnityEngine;
using System.Collections;
using CloudGoods.Services;

namespace CloudGoods.SDK.Models
{
    public class RegisterUserRequest : IRequestClass
    {
        public string AppId;
        public string UserName;
        public string UserEmail;
        public string Password;

        public RegisterUserRequest() { }

        public RegisterUserRequest(string name, string email, string password)
        {
            UserName = name;
            UserEmail = email;
            Password = password;
            AppId = CloudGoodsSettings.AppID;
        }

        public string ToHashable()
        {
            return AppId + UserName + UserEmail + Password;
        }
    }
}
