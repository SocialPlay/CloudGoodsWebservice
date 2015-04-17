using UnityEngine;
using System.Collections;
using CloudGoods.Services;

namespace CloudGoods.SDK.Models
{
    public class RegisterUserRequest : IRequestClass
    {
        public string AppId;
        public string Name;
        public string Email;
        public string Password;

        public RegisterUserRequest(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            AppId = CloudGoodsSettings.AppID;
        }

        public string ToHashable()
        {
            return AppId + Name + Email + Password;
        }
    }
}
