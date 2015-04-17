using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CloudGoods.SDK.Models
{
    public class MoveItemsRequest : IRequestClass
    {
        public List<MoveOrder> MoveOrders = new List<MoveOrder>();
        public AlternateDestinationOwner AlternateDestination = null;

        public string ToHashable()
        {
            string Results = "";
            MoveOrders.ForEach(order => { Results += order.ToHashable(); });
            Results += AlternateDestinationOwner.ToHashable(AlternateDestination);
            return Results;
        }


        public MoveItemsRequest(List<MoveOrder> moveOrders, AlternateDestinationOwner alternateDestination = null)
        {
            MoveOrders = moveOrders;
            AlternateDestination = alternateDestination;
        }

        public MoveItemsRequest(MoveOrder singleMoveOrder, AlternateDestinationOwner alternateDestination = null)
        {
            MoveOrders = new List<MoveOrder>() { singleMoveOrder };
            AlternateDestination = alternateDestination;
        }

        public class MoveOrder
        {
            public string StackId;
            public int Amount;
            public int Location;

            public string ToHashable()
            {
                return StackId + Amount + Location;
            }

            public MoveOrder(string stackId, int amount, int location)
            {
                StackId = stackId;
                Amount = amount;
                Location = location;
            }
        }
    }
}
