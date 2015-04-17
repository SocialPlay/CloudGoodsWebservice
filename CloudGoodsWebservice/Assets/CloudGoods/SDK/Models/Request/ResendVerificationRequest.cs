using UnityEngine;
using System.Collections;
using CloudGoods.Services;

namespace CloudGoods.SDK.Models
{
    public class ResendVerificationRequest : IRequestClass
    {
        public string Email;
        public string AppId;

        public string ToHashable()
        {
            return Email + AppId;
        }

        public ResendVerificationRequest(string email)
        {
            Email = email;
            AppId = CloudGoodsSettings.AppID;
        }
    }

}
