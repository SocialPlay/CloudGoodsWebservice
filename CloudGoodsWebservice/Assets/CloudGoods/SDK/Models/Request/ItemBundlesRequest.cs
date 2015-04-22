using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{

    public class ItemBundlesRequest : IRequestClass
    {
       public TagSelection Tags = null;

        public string ToHashable()
        {
            return Tags != null ? Tags.ToHashable() : "ItemBundles";
        }
        public ItemBundlesRequest(TagSelection tags =null)
        {
            Tags = tags;
        }

    }
}
