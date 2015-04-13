using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;

namespace CloudGoods.SDK.Container.Restrcitions
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
