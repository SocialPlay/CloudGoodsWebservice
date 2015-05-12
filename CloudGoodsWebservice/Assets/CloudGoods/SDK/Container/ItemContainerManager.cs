using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Container
{

    [System.Serializable]
    public class ItemContainerManager
    {

        public static ContainerMoveState.ActionState AddItem(OwnedItemInformation addItem, ItemContainer targetContainer)
        {
            if (addItem.IsLocked)
                return ContainerMoveState.ActionState.No;

            ContainerMoveState targetAddState = targetContainer.GetContainerAddState(addItem);

            switch (targetAddState.ContainerActionState)
            {
                case ContainerMoveState.ActionState.Add:

                    targetContainer.Add(addItem, targetAddState.PossibleAddAmount);

                    break;
                case ContainerMoveState.ActionState.No:
                    break;
                default:
                    break;
            }

            return targetAddState.ContainerActionState;
        }

        public static ContainerMoveState.ActionState MoveItem(OwnedItemInformation movingItemData, ItemContainer targetContainer)
        {
            try
            {
                if (movingItemData.IsLocked)
                    return ContainerMoveState.ActionState.No;

                if (movingItemData == null)
                    throw new Exception("Can Not Move null item");

                if (targetContainer == null)
                    throw new Exception("Can not move item to null container");

                ContainerMoveState targetAddState = targetContainer.GetContainerAddState(movingItemData);

                switch (targetAddState.ContainerActionState)
                {
                    case ContainerMoveState.ActionState.Add:

                        OwnedItemInformation newItemData = new OwnedItemInformation();
                        newItemData.Amount = movingItemData.Amount;
                        newItemData.Information = movingItemData.Information;
                        newItemData.OwnerContainer = movingItemData.OwnerContainer;
                        newItemData.StackLocationId = movingItemData.StackLocationId;
                        newItemData.Location = movingItemData.Location;

                        if (movingItemData.OwnerContainer != null)
                        {
                            if (RemoveItem(movingItemData, movingItemData.OwnerContainer) == ContainerMoveState.ActionState.No)
                                return ContainerMoveState.ActionState.No;
                        }


                        targetContainer.Add(newItemData, targetAddState.PossibleAddAmount);

                        break;
                    case ContainerMoveState.ActionState.No:
                        break;
                    default:
                        break;
                }

                return targetAddState.ContainerActionState;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);

                return ContainerMoveState.ActionState.No;
            }
        }


        public static ContainerMoveState.ActionState RemoveItem(OwnedItemInformation RemoveItemData, ItemContainer TargetContainer)
        {

            if (RemoveItemData.IsLocked)
                return ContainerMoveState.ActionState.No;

            if (TargetContainer.GetContainerRemoveState(RemoveItemData).ContainerActionState == ContainerMoveState.ActionState.Remove)
            {
                TargetContainer.Remove(RemoveItemData, false, RemoveItemData.Amount);
                return ContainerMoveState.ActionState.Remove;
            }

            return ContainerMoveState.ActionState.No;
        }
    }
}

