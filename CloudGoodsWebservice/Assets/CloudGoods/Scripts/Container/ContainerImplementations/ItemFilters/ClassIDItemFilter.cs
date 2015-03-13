using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoodsClasses;

public class ClassIDItemFilter : ContainerItemFilter 
{
    public List<int> ClassIDs;

    override public bool IsItemFilteredIn(ItemData item)
    {
        bool found = false;
        foreach (int classID in ClassIDs)
        {
            if (item.ClassId == classID)
            {
                found= true;
                break;
            }
        }
        if (type== InvertedState.excluded)
        {
            found = !found;
        }
        return found;
    }
}
