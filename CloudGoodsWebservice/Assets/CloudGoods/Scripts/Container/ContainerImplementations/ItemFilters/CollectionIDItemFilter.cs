using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoodsClasses;

public class CollectionIDItemFilter : ItemDataSelector
{
    public override bool isItemSelected(ItemData item, IEnumerable CollectionIDs, bool IsInverted = false)
    {
        foreach (int CollectionID in CollectionIDs)
        {
            if (CollectionID == item.CollectionId)
            {
                if (!IsInverted)
                    return true;
                else
                {
                    return false;
                }
            }
        }

        if (!IsInverted)
            return false;
        else
        {
            return true;
        }
    }
}
