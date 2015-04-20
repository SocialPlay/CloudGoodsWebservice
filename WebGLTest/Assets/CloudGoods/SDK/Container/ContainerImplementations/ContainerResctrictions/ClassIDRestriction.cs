using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;


namespace CloudGoods.SDK.Container.Restrcitions
{
    public class ClassIDRestriction : MonoBehaviour, IContainerRestriction
    {

        public List<int> ClassIDList = new List<int>();
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
                if (ClassIDList.Exists(x => x == itemData.Information.ClassId))
                {
                    Debug.LogWarning("Item Resticted for being added to container because it has a Class ID Restriction");
                    return true;
                }

                return false;
            }
            else
            {
                if (ClassIDList.Exists(x => x == itemData.Information.ClassId))
                    return false;

                Debug.LogWarning("Item Resticted for being added to container because it has a Class ID Restriction");
                return true;
            }
        }
    }
}