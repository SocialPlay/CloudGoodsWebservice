using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class BundlePurchaseRequest : IRequestClass
    {
        public string payload;

        public string ToHashable()
        {
            return payload;
        }
    }
}
