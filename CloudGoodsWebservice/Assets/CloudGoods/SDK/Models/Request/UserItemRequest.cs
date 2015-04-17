using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Models
{
    public class OwnerItemRequest : IRequestClass
    {
        public int ItemId;
        public int Location;

        public string ToHashable()
        {
            return ItemId.ToString() + Location;
        }

        public OwnerItemRequest(int itemId, int location)
        {
            ItemId = itemId;
            Location = location;
        }
    }
}
