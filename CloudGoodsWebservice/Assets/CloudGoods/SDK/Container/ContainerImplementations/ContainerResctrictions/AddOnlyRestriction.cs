using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
namespace CloudGoods.SDK.Container.Restrcitions
{
    public class AddOnlyRestriction : MonoBehaviour, IContainerRestriction
    {

        ItemContainer restrictedContainer;

        void Awake()
        {
            restrictedContainer = GetComponent<ItemContainer>();
            restrictedContainer.ContainerRemoveRestrictions.Add(this);
        }

        public bool IsRestricted(ContainerAction containerAction, InstancedItemInformation itemData)
        {
            if (containerAction == ContainerAction.remove)
            {
                Debug.LogWarning("Item Resticted for being removed from container because it has an Add-Only Restriction");
                return true;
            }
            else
                return false;
        }
    }
}
