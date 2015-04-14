using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container.Restrcitions;

namespace CloudGoods.SDK.Container.Restrcitions
{

    public class ItemLimitRestriction : MonoBehaviour, IContainerRestriction
    {

        public int ContainerItemLimit = 0;
        public ItemContainer RestrictedContainer;

        void Awake()
        {
            RestrictedContainer = GetComponent<ItemContainer>();
            RestrictedContainer.ContainerAddRestrictions.Add(this);
        }

        public bool IsRestricted(ContainerAction containerAction, InstancedItemInformation itemData)
        {
            if (containerAction == ContainerAction.add)
            {
                if (RestrictedContainer.containerItems.Count >= ContainerItemLimit)
                {
                    Debug.LogWarning("Item Resticted for being added to container because it has a Item Limit Restriction");
                    return true;
                }
            }

            return false;
        }
    }
}
