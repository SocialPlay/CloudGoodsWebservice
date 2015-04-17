using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class ItemBundleInfo
    {
        public int Id;
        public string Name;
        public string Description;
        public string Image;
        public int PremiumPrice;
        public int StandardPrice;
        public List<BundleItemInformation> Items = new List<BundleItemInformation>();
        public int State;

        public class BundleItemInformation
        {
            public int Amount;
            public ItemInformation Information;

            public BundleItemInformation()
            {
                Information = new ItemInformation();
            }
        }
    }
}
