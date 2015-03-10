using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;


[TestFixture]
public class ItemContainerManagerTests : MonoBehaviour {

    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void AddItem_BasicAdd_ReturnsStateAdd()
    {

    }

    [Test]
    public void AddItem_InvalidForContainer_ReturnsStateNo()
    {

    }

    [Test]
    public void AddItem_ItemLocked_ReturnsStateNo()
    {

    }

    [Test]
    public void MoveItem_InvalidRemoveItem_ReturnsStateNo()
    {

    }

    [Test]
    public void MoveItem_ValidRemoveItemInvalidAddItem_ReturnsStateNo()
    {

    }

    [Test]
    public void MoveItem_ValidRemoveItemValidAddItem_ReturnsStateAdd()
    {

    }

    [Test]
    public void MoveItem_ItemLockedReturnsStateNo()
    {

    }

    [Test]
    public void RemoveItem_InvalidRemoveItem_ReturnsStateNo()
    {

    }

    [Test]
    public void RemoveItem_ValidRemoveItem_ReturnsStateRemove()
    {

    }


    [Test]
    public void RemoveItem_ItemLocked_ReturnsStateNo()
    {

    }

    public void SetupContainerWithRestricton()
    {

    }

    public ItemData CreateItemData(int amount, int classId, int collectionId, int energy, int id, int location, string name, string stackLocationId)
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
            StackLocationId = stackLocationId
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
