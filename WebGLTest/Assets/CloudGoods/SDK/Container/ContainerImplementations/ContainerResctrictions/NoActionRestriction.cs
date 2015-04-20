using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container.Restrcitions;

namespace CloudGoods.SDK.Container.Restrcitions
{
    public class NoActionRestriction : MonoBehaviour, IContainerRestriction
    {

        ItemContainer restrictedContainer;

        void Awake()
        {
            restrictedContainer = GetComponent<ItemContainer>();
            restrictedContainer.ContainerRemoveRestrictions.Add(this);
            restrictedContainer.ContainerAddRestrictions.Add(this);
        }

        public bool IsRestricted(ContainerAction containerAction, InstancedItemInformation itemData)
        {
            Debug.LogWarning("Item Resticted for being added to or removed from container because it has a No Action Restriction");
            return true;
        }
    }
}
