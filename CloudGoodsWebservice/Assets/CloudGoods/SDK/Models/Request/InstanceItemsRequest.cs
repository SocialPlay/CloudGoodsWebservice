using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class InstanceItemsRequest : IRequestClass
    {
        public int InstanceId;
        public int Location;
        public List<string> AndTags;
        public List<string> OrTags;

        public string ToHashable()
        {
            string hashable = InstanceId.ToString() + Location.ToString();
            if (AndTags != null)
                AndTags.ForEach(tag => { hashable += tag; });
            if (OrTags != null)
                OrTags.ForEach(tag => { hashable += tag; });
            return hashable;
        }

        public InstanceItemsRequest(int instanceId, int location, List<string> andTags = null, List<string> orTags = null)
        {
            InstanceId = instanceId;
            Location = location;
            AndTags = andTags;
            OrTags = orTags;
        }
    }
}
