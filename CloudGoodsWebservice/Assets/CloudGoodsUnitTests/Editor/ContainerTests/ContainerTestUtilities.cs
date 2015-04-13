using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container;
using CloudGoods.SDK.Container.Restrcitions;

public class ContainerTestUtilities
{

    public ItemContainer SetUpContainer(bool isRestrictedAdd, bool isRestrictedRemove, List<ItemContainer> listContainer = null)
    {
        GameObject containerObj = new GameObject();
        containerObj.name = "Item Container";
        ItemContainer container = containerObj.AddComponent<ItemContainer>();
        container.ContainerAddAction = containerObj.AddComponent<BasicAddContainer>();
        BasicAddContainer containerAdd = (BasicAddContainer)container.ContainerAddAction;
        containerAdd.ItemContainer = container;


        ClassIDRestriction restriction = containerObj.AddComponent<ClassIDRestriction>();
        restriction.ClassIDList.Add(10);
        restriction.IsExcluded = true;

        if (isRestrictedAdd)
            container.ContainerAddRestrictions.Add(restriction);

        if (isRestrictedRemove)
            container.ContainerRemoveRestrictions.Add(restriction);

        if (listContainer != null)
            listContainer.Add(container);

        return container;
    }

    public OwnedItemInformation CreateItemData(int amount, int classId, int collectionId, int energy, int id, int location, string name, string stackLocationId, bool isLocked)
    {
        OwnedItemInformation tmpData = new OwnedItemInformation()
        {
            Amount = amount,
            Location = location,
            StackLocationId = stackLocationId,
            IsLocked = isLocked,
            Information = new ItemInformation
            {
                ClassId = classId,
                CollectionId = collectionId,
                Detail = "Some Details Here",
                Energy = energy,
                Id = id,
                Name = name
            }
        };

        for (int i = 0; i < 3; i++)
        {
            tmpData.Information.Behaviours.Add(new ItemInformation.Behaviour() { Name = i.ToString(), Id = i });
        }

        for (int i = 0; i < 3; i++)
        {
            tmpData.Information.Tags.Add(new ItemInformation.Tag() { Name = i.ToString(), Id = i });
        }

        return tmpData;
    }
}
