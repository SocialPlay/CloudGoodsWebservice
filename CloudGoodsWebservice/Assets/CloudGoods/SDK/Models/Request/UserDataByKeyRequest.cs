using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class UserDataByKeyRequest : IRequestClass
    {
        public string Key;
        public string ToHashable()
        {
            return Key;
        }

        public UserDataByKeyRequest(string key)
        {
            Key = key;
        }
    }
}
