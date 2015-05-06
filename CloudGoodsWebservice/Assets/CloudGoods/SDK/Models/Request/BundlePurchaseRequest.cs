using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class BundlePurchaseRequest : IRequestClass
    {
        public int BundleID;
        public int PaymentPlatform;
        public string ReceiptToken;

        public string ToHashable()
        {
            return BundleID + PaymentPlatform + ReceiptToken;
        }
    }
}
