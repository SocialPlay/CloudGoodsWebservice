using UnityEngine;
using System.Collections;
using CloudGoods.Services;

namespace CloudGoods.SDK.Models
{
    public class ForgotPasswordRequest : IRequestClass
    {
        public string Email;
        public string AppId;

        public string ToHashable()
        {
            return Email + AppId;
        }

        public ForgotPasswordRequest(string email)
        {
            Email = email;
            AppId = CloudGoodsSettings.AppID;
        }
    }
}
