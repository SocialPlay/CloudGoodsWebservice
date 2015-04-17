using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Container;

namespace CloudGoods.SDK.Models
{
    public class OwnedItemInformation : InstancedItemInformation
    {
        public ItemContainer OwnerContainer;
        public bool IsLocked = false;

        public bool IsSameItemAs(InstancedItemInformation other)
        {
            if (this == null || other == null)
            {
                return false;
            }
            if (Information.Id == other.Information.Id)
                return true;
            else return false;
        }
    }
}