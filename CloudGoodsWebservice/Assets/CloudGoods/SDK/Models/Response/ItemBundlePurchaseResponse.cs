using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class ItemBundlePurchaseResponse
    {
        public int StatusCode;
        public SimpleItemInfo StandardCurrency;
        public int PremiumBalance;

        public List<SimpleItemInfo> purchasedItems = new List<SimpleItemInfo>();
    }
}
