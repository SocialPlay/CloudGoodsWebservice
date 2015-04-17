using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{

    public class ItemBundlesRequest : IRequestClass
    {
       public List<string> AndTags = null;
      public  List<string> OrTags = null;

        public string ToHashable()
        {
            string hashable = string.Empty;
            AndTags.ForEach(tag => hashable += tag);
            OrTags.ForEach(tag => hashable += tag);
            return hashable;
        }
        public ItemBundlesRequest(List<string> andTags = null, List<string> orTags = null)
        {
            AndTags = andTags;
            OrTags = orTags;
        }

    }
}
