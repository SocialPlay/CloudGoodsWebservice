using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class BundleItem
    {
        public int Quantity;
        public int Quality;

        public string Name;
        public string Image;
        public string Description;

        public List<BundleItemDetails> bundleItemDetails = new List<BundleItemDetails>();
    }
}