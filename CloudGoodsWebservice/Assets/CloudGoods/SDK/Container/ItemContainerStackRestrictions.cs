using UnityEngine;
using System.Collections;

namespace CloudGoods.SDK.Container
{
    [System.Serializable]
    public class ItemContainerStackRestrictions
    {
        public int RestrictionType;
        public int RestrictionAmount;

        public virtual int GetRestrictionForType(int type)
        {
            if (RestrictionType == type || RestrictionType == -1)
            {
                return RestrictionAmount;
            }
            return -1;
        }
    }
}
