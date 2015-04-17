using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Container
{
    [System.Serializable]
    public class ContainerMoveState
    {
        public enum ActionState
        {
            Add, Swap, No, Remove
        }

        public ActionState ContainerActionState;
        public int PossibleAddAmount;
        public ItemInformation PossibleSwapItem;

        public ContainerMoveState(ActionState newState = ActionState.No, int possibleAddAmount = 0, ItemInformation possibleSwapItem = null)
        {
            this.ContainerActionState = newState;
            this.PossibleAddAmount = possibleAddAmount;
            this.PossibleSwapItem = possibleSwapItem;
        }
    }
}
