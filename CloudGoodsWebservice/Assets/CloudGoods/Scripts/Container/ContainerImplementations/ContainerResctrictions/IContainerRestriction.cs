using UnityEngine;
using System.Collections;

namespace CloudGoods.Container.Restrcitions
{
    public interface IContainerRestriction
    {

        bool IsRestricted(ContainerAction action, CloudGoods.Models.ItemData itemData);
    }

    public enum ContainerAction
    {
        add,
        remove
    }
}
