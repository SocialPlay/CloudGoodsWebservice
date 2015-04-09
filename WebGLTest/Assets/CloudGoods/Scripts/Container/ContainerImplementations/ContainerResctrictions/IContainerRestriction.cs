using UnityEngine;
using System.Collections;
using CloudGoods.Models;

namespace CloudGoods.Container.Restrcitions
{
    public interface IContainerRestriction
    {

        bool IsRestricted(ContainerAction action, InstancedItemInformation itemData);
    }

    public enum ContainerAction
    {
        add,
        remove
    }
}
