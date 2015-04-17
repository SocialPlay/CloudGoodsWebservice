using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.Enums;


namespace CloudGoods.SDK.Models
{

    public class ItemBundle
    {
        public int ID;
        public int CreditPrice;
        public int CoinPrice;

        public CloudGoodsBundle State;

        public string Name;
        public string Description;
        public string Image;

        public List<BundleItem> bundleItems = new List<BundleItem>();
    }
}
