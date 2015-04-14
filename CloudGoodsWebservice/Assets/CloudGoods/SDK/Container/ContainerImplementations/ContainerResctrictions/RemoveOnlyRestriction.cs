using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container.Restrcitions;

namespace CloudGoods.SDK.Container.Restrcitions
{

    public class RemoveOnlyRestriction : MonoBehaviour, IContainerRestriction
    {

        ItemContainer restrictedContainer;

        void Awake()
        {
            restrictedContainer = GetComponent<ItemContainer>();
            restrictedContainer.ContainerAddRestrictions.Add(this);
        }

        public bool IsRestricted(ContainerAction containerAction, InstancedItemInformation itemData)
        {
            if (containerAction == ContainerAction.add)
            {
                Debug.LogWarning("Item Resticted for being added to container because it has a Remove Only Restriction");
                return true;
            }
            else
                return false;
        }
    }
}
