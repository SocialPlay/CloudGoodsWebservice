using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ItemContainerManager
{

    public static ContainerMoveState.ActionState AddItem(ItemData addItem, ItemContainer targetContainer)
    {
        if (addItem.IsLocked)
            return ContainerMoveState.ActionState.No;

        ContainerMoveState targetAddState = targetContainer.GetContainerAddState(addItem);

        switch (targetAddState.actionState)
        {
            case ContainerMoveState.ActionState.Add:

                targetContainer.Add(addItem, targetAddState.possibleAddAmount);

                break;
            case ContainerMoveState.ActionState.No:
                break;
            default:
                break;
        }

        return targetAddState.actionState;
    }

    public static ContainerMoveState.ActionState MoveItem(ItemData movingItemData, ItemContainer targetContainer)
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

            switch (targetAddState.actionState)
            {
                case ContainerMoveState.ActionState.Add:

                    ItemData newItemData = movingItemData;

                    if (movingItemData.OwnerContainer != null)
                    {
                        if (RemoveItem(movingItemData, movingItemData.OwnerContainer) == ContainerMoveState.ActionState.No)
                            return ContainerMoveState.ActionState.No;
                    }

                    targetContainer.Add(newItemData, targetAddState.possibleAddAmount);

                    break;
                case ContainerMoveState.ActionState.No:
                    break;
                default:
                    break;
            }

            return targetAddState.actionState;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);

            return ContainerMoveState.ActionState.No;
        }
    }


    public static ContainerMoveState.ActionState RemoveItem(ItemData RemoveItemData, ItemContainer TargetContainer)
    {

        if (RemoveItemData.IsLocked)
            return ContainerMoveState.ActionState.No;

        if (TargetContainer.GetContainerRemoveState(RemoveItemData) == false)
        {
            return ContainerMoveState.ActionState.No;
        }

        TargetContainer.Remove(RemoveItemData, false, RemoveItemData.Amount);

        return ContainerMoveState.ActionState.Remove;
    }
}

