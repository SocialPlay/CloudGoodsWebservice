using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
using System.Collections.Generic;

namespace CloudGoods.SDK.Container
{
    [System.Serializable]
    public abstract class ItemStackRestrictionHandler
    {
        protected List<ItemContainerStackRestrictions> restrictions = new List<ItemContainerStackRestrictions>();


        public virtual int GetRestrictedAmount(ItemInformation data, ItemContainer target)
        {
            restrictions = GetRestrictionsFor(target);
            foreach (ItemContainerStackRestrictions restriction in restrictions)
            {
                int restrictedAmount = restriction.GetRestrictionForType(data.ClassId);
                if (restrictedAmount != -1)
                {
                    return restrictedAmount;
                }
            }
            return -1;
        }

        protected virtual List<ItemContainerStackRestrictions> GetRestrictionsFor(ItemContainer target)
        {
            return restrictions;
        }
    }
}
