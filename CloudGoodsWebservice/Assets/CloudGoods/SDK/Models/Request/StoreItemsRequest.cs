using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class StoreItemsRequest : IRequestClass
    {
        public TagSelection Tags = null;

        public string ToHashable()
        {
            return Tags != null ? Tags.ToHashable() : "StoreItems";
        }

        public StoreItemsRequest(TagSelection tags = null)
        {
            Tags = tags;
        }
    }
}
