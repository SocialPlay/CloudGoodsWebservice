using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{

    public class UpdateItemsByIdsRequest : IRequestClass
    {
        public List<UpdateOrderByID> Orders = new List<UpdateOrderByID>();
        public AlternateDestinationOwner AlternateDestination;

        public string ToHashable()
        {
            string resluts = string.Empty;
            Orders.ForEach(x => resluts += x.ToHashable());
            resluts += AlternateDestinationOwner.ToHashable(AlternateDestination);
            return resluts;
        }

        public UpdateItemsByIdsRequest(int itemId, int amount, int location, AlternateDestinationOwner alternateDestination = null)
        {
            Orders = new List<UpdateOrderByID>() { new UpdateOrderByID(itemId, amount, location) };
            AlternateDestination = alternateDestination;
        }

        public UpdateItemsByIdsRequest(List<UpdateOrderByID> orders, AlternateDestinationOwner alternateDestination = null)
        {
            Orders = orders;
            AlternateDestination = alternateDestination;
        }


        public class UpdateOrderByID : IRequestClass
        {
            public int ItemId;
            public int Amount;
            public int Location;

            public string ToHashable()
            {
                return ItemId.ToString() + Amount.ToString() + Location.ToString();
            }

            public UpdateOrderByID(int itemId, int amount, int location)
            {
                ItemId = itemId;
                Amount = amount;
                Location = location;
            }
        }
    }
}