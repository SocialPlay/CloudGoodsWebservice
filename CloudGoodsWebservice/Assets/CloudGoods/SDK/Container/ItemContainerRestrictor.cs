using System;
using CloudGoods.SDK.Container.Restrcitions;
using UnityEngine;

namespace CloudGoods.SDK.Container
{

    public class ItemContainerRestrictor : MonoBehaviour
    {
        public enum RestrictorState
        {
            Normal,
            AddOnly,
            RemoveOnly,
            NoAction
        }

        public RestrictorState ContainerRestrictorState = RestrictorState.Normal;

        public ItemContainer RestrictedContainer;

        void Awake()
        {
            CheckForValidRestrictedContainer();
        }

        public void CheckForValidRestrictedContainer()
        {
            if (!RestrictedContainer)
                throw new Exception("ItemContainerRestrictor could not find a container to restrict.");
        }


        public bool IsRestricted(ContainerAction action)
        {
            switch (ContainerRestrictorState)
            {
                case RestrictorState.Normal:
                    return false;
                case RestrictorState.AddOnly:
                    if (action == ContainerAction.add)
                    {
                        return false;
                    }
                    return true;
                case RestrictorState.RemoveOnly:
                    if (action == ContainerAction.remove)
                    {
                        return false;
                    }
                    return true;
                case RestrictorState.NoAction:
                    return true;
            }
            return true;
        }
    }
}