using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class InstanceItemsRequest : IRequestClass
    {
        public int InstanceId;
        public int Location;
        public TagSelection Tags = null;

        public string ToHashable()
        {
            return InstanceId.ToString() + Location.ToString() + (Tags != null ? Tags.ToHashable() : "");
        }   

        public InstanceItemsRequest(int instanceId, int location, TagSelection tags = null)
        {
            InstanceId = instanceId;
            Location = location;
            Tags = tags;
        }
    }
}
