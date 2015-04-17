using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class RedeemItemVouchersRequest : IRequestClass
    {
        public List<ItemVoucherSelection> SelectedVouchers;
        public AlternateDestinationOwner AlternateDestination { get; set; }

        public string ToHashable()
        {
            string HashValue = AlternateDestinationOwner.ToHashable(AlternateDestination);
            SelectedVouchers.ForEach(v => HashValue +=v.ToHashable());
            return HashValue;
        }

        public RedeemItemVouchersRequest(List<ItemVoucherSelection> selectedVouchers, AlternateDestinationOwner alternateDestination = null)
        {
            SelectedVouchers = selectedVouchers;
            AlternateDestination = alternateDestination;
        }

        public RedeemItemVouchersRequest(int voucherId, int itemId, int amount, int location = 0, AlternateDestinationOwner alternateDestination = null)
        {
            SelectedVouchers = new List<ItemVoucherSelection>() { new ItemVoucherSelection(voucherId, itemId, amount, location) };
            AlternateDestination = alternateDestination;
        }

        public class ItemVoucherSelection : IRequestClass
        {
            public int VoucherId;
            public int ItemId;
            public int Amount;
            public int Location;

            public string ToHashable()
            {
                return VoucherId.ToString() + ItemId.ToString() + Amount.ToString() + Location.ToString();
            }

            public ItemVoucherSelection(int voucherId, int itemId, int amount, int location)
            {
                VoucherId = voucherId;
                ItemId = itemId;
                Amount = amount;
                Location = location;
            }
        }


    }
}
