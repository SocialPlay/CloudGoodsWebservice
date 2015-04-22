using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class UserItemsRequest : IRequestClass
    {
        public int Location;
        public TagSelection Tags;

        public string ToHashable()
        {
            return Location.ToString() + (Tags != null ? Tags.ToHashable() : "");
        }

        public UserItemsRequest(int location, TagSelection tags = null)
        {
            Location = location;
            Tags = tags;
        }
    }
}
