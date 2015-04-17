using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class BundlePurchaseRequest
    {
        public int BundleID;
        public string UserID;
        public string ReceiptToken;
        public int PaymentPlatform;
    }
}
