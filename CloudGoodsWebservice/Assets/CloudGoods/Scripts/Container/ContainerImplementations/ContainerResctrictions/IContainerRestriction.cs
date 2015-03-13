using UnityEngine;
using System.Collections;

public interface IContainerRestriction {

    bool IsRestricted(ContainerAction action, CloudGoodsClasses.ItemData itemData);
}

public enum ContainerAction
{
    add,
    remove
}
