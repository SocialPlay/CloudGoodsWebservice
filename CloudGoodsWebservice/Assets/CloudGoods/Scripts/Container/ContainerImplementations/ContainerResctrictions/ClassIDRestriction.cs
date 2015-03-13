using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassIDRestriction : MonoBehaviour, IContainerRestriction {

    public List<int> classIDList = new List<int>();
    public bool IsExcluded = false;

    ItemContainer restrictedContainer;

    void Awake()
    {
        restrictedContainer = GetComponent<ItemContainer>();
        restrictedContainer.containerAddRestrictions.Add(this);
    }

    public bool IsRestricted(ContainerAction action, ItemData itemData)
    {
        if (IsExcluded)
        {
            if (classIDList.Exists(x => x == itemData.ClassId))
            {
                Debug.LogWarning("Item Resticted for being added to container because it has a Class ID Restriction");
                return true;
            }

            return false;
        }
        else
        {
            if (classIDList.Exists(x => x == itemData.ClassId))
                return false;
           
            Debug.LogWarning("Item Resticted for being added to container because it has a Class ID Restriction");
            return true;
        }
    }
}
