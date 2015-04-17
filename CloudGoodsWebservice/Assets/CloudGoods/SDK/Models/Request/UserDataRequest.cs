using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{
    public class UserDataRequest : IRequestClass
    {
        public string Key;

        public string ToHashable()
        {
            return Key;
        }

        public UserDataRequest(string key)
        {
            Key = key;
        }
    }
}
