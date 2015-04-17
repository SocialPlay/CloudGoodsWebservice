using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{

    public class AppDataUpdateRequest : IRequestClass
    {
        public string Key;
        public string Value;

        public string ToHashable()
        {
            return Key + Value;
        }
        public AppDataUpdateRequest(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
