using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class SessionItemsRequest : IRequestClass
    {
        public int Location;
        public TagSelection Tags = null;
        public string ToHashable()
        {
            return Location.ToString() + (Tags != null ? Tags.ToHashable() : "");
        }

        public SessionItemsRequest(int location, TagSelection tags=null)
        {
            Location = location;
            Tags = tags;
        }
    }
}