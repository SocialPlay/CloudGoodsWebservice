using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class SessionItemsRequest : IRequestClass
    {
        public int Location;
        public List<string> AndTags;
        public List<string> OrTags;

        public string ToHashable()
        {
            string hashable = Location.ToString();      
            AndTags.ForEach(tag => { hashable += tag; });       
            OrTags.ForEach(tag => { hashable += tag; });
            return hashable;
        }

        public SessionItemsRequest(int location, List<string> andTags = null, List<string> orTags = null)
        {
            Location = location;
            AndTags = andTags;
            OrTags = orTags;
        }
    }
}