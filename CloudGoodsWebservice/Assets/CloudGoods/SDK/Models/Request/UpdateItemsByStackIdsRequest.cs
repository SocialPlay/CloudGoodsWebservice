using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CloudGoods.SDK.Models
{
    public class UpdateItemsByStackIdRequest : IRequestClass
    {
        public List<UpdateOrderByStackId> Orders = new List<UpdateOrderByStackId>();
        public AlternateDestinationOwner AlternateDestination;

        public string ToHashable()
        {
            string resluts = "";
            Orders.ForEach(x => resluts += x.ToHashable());
            resluts += AlternateDestinationOwner.ToHashable(AlternateDestination);
            return resluts;
        }

        public class UpdateOrderByStackId : IRequestClass
        {
            public string stackId;
            public int amount;
            public int location;

            public string ToHashable()
            {
                return stackId + amount + location;
            }
        }
    }
}
