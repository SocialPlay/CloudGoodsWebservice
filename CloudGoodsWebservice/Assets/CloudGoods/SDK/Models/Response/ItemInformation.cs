using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class ItemInformation
    {
        public int Id;
        public int CollectionId;
        public int ClassId;
        public string Name;
        public string Detail;
        public int Energy;
        public int Quality;
        public string Description;
        public string ImageName;
        public string AssetBundleURL;
        public List<Behaviour> Behaviours = new List<Behaviour>();
        public List<Tag> Tags = new List<Tag>();

        public class Tag
        {
            public string Name;
            public int Id;
        }

        public class Behaviour
        {
            public string Name;
            public int Id;
        }

    }
}
