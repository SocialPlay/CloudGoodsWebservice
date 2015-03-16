using UnityEngine;
using System.Collections;

public interface IContainerRestriction {

    bool IsRestricted(ContainerAction action, CallHandler.Models.ItemData itemData);
}

public enum ContainerAction
{
    add,
    remove
}
