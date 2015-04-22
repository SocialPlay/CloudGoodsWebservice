using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class TagSelection : IRequestClass
    {
        public List<string> AndTags = new List<string>();
        public List<string> OrTags = new List<string>();
        public List<string> NotTags = new List<string>();

        public string ToHashable()
        {
            string hashable = string.Empty;
            AndTags.ForEach(tag => hashable += tag);
            OrTags.ForEach(tag => hashable += tag);
            NotTags.ForEach(tag => hashable += tag);
            return hashable;
        }     

    }
}
