using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoodsClasses;

public class TagItemFilter : ContainerItemFilter
{

    public List<string> Tags = new List<string>();

    override public bool IsItemFilteredIn(ItemData item)
    {
        bool found = false;
        foreach (string tag in Tags)
        {
            if (item.Tags.Exists(x => x.Name == tag))
            {
                found = true;
            }
        }
        if (type == InvertedState.excluded)
        {
            found = !found;
        }
        return found;
    }
}
