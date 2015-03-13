using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerTestUtilities  {

    public ItemContainer SetUpContainer(bool isRestrictedAdd, bool isRestrictedRemove, List<ItemContainer> listContainer = null)
    {
        GameObject containerObj = new GameObject();
        containerObj.name = "Item Container";
        ItemContainer container = containerObj.AddComponent<ItemContainer>();
        container.containerAddAction = containerObj.AddComponent<BasicAddContainer>();
        BasicAddContainer containerAdd = (BasicAddContainer)container.containerAddAction;
        containerAdd.itemContainer = container;


        ClassIDRestriction restriction = containerObj.AddComponent<ClassIDRestriction>();
        restriction.classIDList.Add(10);
        restriction.IsExcluded = true;

        if (isRestrictedAdd)
            container.containerAddRestrictions.Add(restriction);

        if (isRestrictedRemove)
            container.containerRemoveRestrictions.Add(restriction);

        if (listContainer != null)
            listContainer.Add(container);

        return container;
    }

    public ItemData CreateItemData(int amount, int classId, int collectionId, int energy, int id, int location, string name, string stackLocationId, bool isLocked)
    {
        ItemData tmpData = new ItemData()
        {
            amount = amount,
            classId = classId,
            collectionId = collectionId,
            detail = "Some Details Here",
            energy = energy,
            Id = id,
            location = location,
            name = name,
            stackLocationId = stackLocationId,
            IsLocked = isLocked
        };

        for (int i = 0; i < 3; i++)
        {
            tmpData.behaviours.Add(new ItemData.Behaviours() { name = i.ToString(), Id = i });
        }

        for (int i = 0; i < 3; i++)
        {
            tmpData.tags.Add(new ItemData.Tag() { name = i.ToString(), Id = i });
        }

        return tmpData;
    }
}
