using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class ItemVoucherRequest : IRequestClass
    {
        public int VoucherId;

        public string ToHashable()
        {
            return VoucherId.ToString();
        }

        public ItemVoucherRequest(int voucherId)
        {
            VoucherId = voucherId;
        }
    }
}
