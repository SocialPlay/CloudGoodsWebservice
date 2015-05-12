using CloudGoods.SDK.Container;
using CloudGoods.SDK.Item;
using CloudGoods.SDK.Models;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnityUIItemDropComponent : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public void OnDrop(PointerEventData data)
	{
        if (data != null && data.pointerDrag.GetComponent<ItemDataComponent>() != null)
        {
            var originalObj = data.pointerDrag;
            if (originalObj == null)
                return;

            if (originalObj.GetComponent<ItemDataComponent>().itemData.OwnerContainer != GetComponent<ItemContainer>())
                ItemContainerManager.MoveItem(originalObj.GetComponent<ItemDataComponent>().itemData, GetComponent<ItemContainer>());
        }
	}

	public void OnPointerEnter(PointerEventData data)
	{
	}

	public void OnPointerExit(PointerEventData data)
	{
	}
	
}
