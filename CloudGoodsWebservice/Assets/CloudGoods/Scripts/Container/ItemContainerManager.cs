using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ItemContainerManager
{
    public static ContainerMoveState.ActionState MoveItem(ItemData movingItemData, ItemContainer lastContainer, ItemContainer targetContainer)
    {
        try
        {
            if (movingItemData == null)
                throw new Exception("Can Not Move null item");

            if (targetContainer == null)
                throw new Exception("Can not move item to null container");

            if (lastContainer != null)
            {
                Debug.Log(lastContainer.GetContainerRemoveState(movingItemData));
                if (lastContainer.GetContainerRemoveState(movingItemData) == false)
                {
                    return ContainerMoveState.ActionState.No;
                }
            }

            ContainerMoveState targetAddState = targetContainer.GetContainerAddState(movingItemData);

            switch (targetAddState.actionState)
            {
                case ContainerMoveState.ActionState.Add:

                    ItemData newItemData = new ItemData()
                        {

                        };

                    if (movingItemData.OwnerContainer != null)
                    {
                        movingItemData.OwnerContainer.Remove(movingItemData, true, targetAddState.possibleAddAmount);
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


    public static ContainerMoveState.ActionState RemoveItem(ItemContainer container, ItemData itemData)
    {
        return ContainerMoveState.ActionState.Remove;
    }
}

