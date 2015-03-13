using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerTestUtilities  {

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

    public ItemData CreateItemData(int amount, int classId, int collectionId, int energy, int id, int location, string name, string stackLocationId, bool isLocked)
    {
        ItemData tmpData = new ItemData()
        {
            Amount = amount,
            ClassId = classId,
            CollectionId = collectionId,
            Detail = "Some Details Here",
            Energy = energy,
            Id = id,
            Location = location,
            Name = name,
            StackLocationId = stackLocationId,
            IsLocked = isLocked
        };

        for (int i = 0; i < 3; i++)
        {
            tmpData.Behaviours.Add(new ItemData.Behaviour() { Name = i.ToString(), Id = i });
        }

        for (int i = 0; i < 3; i++)
        {
            tmpData.Tags.Add(new ItemData.Tag() { Name = i.ToString(), Id = i });
        }

        return tmpData;
    }
}
