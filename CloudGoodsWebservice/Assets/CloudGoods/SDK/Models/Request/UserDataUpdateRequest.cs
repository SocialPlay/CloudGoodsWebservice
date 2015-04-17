using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class UserDataUpdateRequest : IRequestClass
    {
        public string Key;
        public string Value;

        public string ToHashable()
        {
            return Key + Value;
        }

        public UserDataUpdateRequest(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
