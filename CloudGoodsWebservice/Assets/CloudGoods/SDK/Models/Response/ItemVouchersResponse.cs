using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace CloudGoods.SDK.Models
{
    public class ItemVouchersResponse
    {
        public List<VoucherItemInformation> Vouchers;
    }

    public class VoucherItemInformation
    {
        public int VoucherId;
        public int Amount;
        public ItemInformation Information;

        public VoucherItemInformation()
        {
            Information = new ItemInformation();
        }
    }
}
