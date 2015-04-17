using UnityEngine;
using System.Collections;
namespace CloudGoods.SDK.Models
{

    public class AppDataRequest : IRequestClass
    {
        public string Key;
        public string ToHashable()
        {
         return Key;  
        }

        public AppDataRequest(string key)
        {
            Key = key;
        }
    }
}
