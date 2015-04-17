using UnityEngine;
using System.Collections;


namespace CloudGoods.SDK.Models
{
    public class ItemBundlePurchaseRequest : IRequestClass
    {
        public int BundleID;
        public int PaymentType;
        public int Location;

        public string ToHashable()
        {
            return BundleID.ToString() + PaymentType.ToString() + Location;
        }

        public ItemBundlePurchaseRequest(int bundleId, int paymentType, int location)
        {
            BundleID = bundleId;
            PaymentType = paymentType;
            Location = location;
        }
    }
}

