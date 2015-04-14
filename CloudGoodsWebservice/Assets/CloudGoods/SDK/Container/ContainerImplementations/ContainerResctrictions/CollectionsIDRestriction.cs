using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Container.Restrcitions
{
    public class CollectionsIDRestriction : MonoBehaviour, IContainerRestriction
    {

        public List<int> CollectionsIDList = new List<int>();
        public bool IsExcluded = false;

        ItemContainer restrictedContainer;

        void Awake()
        {
            restrictedContainer = GetComponent<ItemContainer>();
            restrictedContainer.ContainerAddRestrictions.Add(this);
        }

        public bool IsRestricted(ContainerAction action, InstancedItemInformation itemData)
        {
            if (IsExcluded)
            {
                if (CollectionsIDList.Exists(x => x == itemData.Information.CollectionId))
                {
                    Debug.LogWarning("Item Resticted for being added to container because it has a Collection ID Restriction");
                    return true;

                }

                return false;
            }
            else
            {
                if (CollectionsIDList.Exists(x => x == itemData.Information.CollectionId))
                    return false;

                Debug.LogWarning("Item Resticted for being added to container because it has a Collection ID Restriction");
                return true;
            }
        }
    }
}
