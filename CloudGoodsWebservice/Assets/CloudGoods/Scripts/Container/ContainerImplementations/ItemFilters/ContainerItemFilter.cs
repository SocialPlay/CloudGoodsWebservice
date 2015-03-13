using UnityEngine;
using System.Collections;
using CloudGoodsClasses;

public abstract class ContainerItemFilter : MonoBehaviour
{
    public enum InvertedState
    {
        required, excluded
    }

    public InvertedState type;

    public abstract bool IsItemFilteredIn(ItemData item);
}
