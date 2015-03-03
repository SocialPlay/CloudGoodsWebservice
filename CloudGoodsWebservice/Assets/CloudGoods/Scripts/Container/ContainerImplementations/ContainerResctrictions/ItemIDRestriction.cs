using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemIDRestriction : MonoBehaviour, IContainerRestriction {

    public List<int> ItemIDList = new List<int>();
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
            if (ItemIDList.Exists(x => x == itemData.Id))
            {
                Debug.LogWarning("Item Resticted for being added to container because it has a Item ID Restriction");
                return true;
            }

            return false;
        }
        else
        {
            if (ItemIDList.Exists(x => x == itemData.Id))
                return false;
           
            Debug.LogWarning("Item Resticted for being added to container because it has a Item ID Restriction");
            return true;
        }
    }
}
