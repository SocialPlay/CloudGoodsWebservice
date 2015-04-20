using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container;

[TestFixture]
public class ItemContainerManagerTests : MonoBehaviour
{

    List<ItemContainer> ItemContainers;
   public List<OwnedItemInformation> ItemDatas;
    ContainerTestUtilities containerTestUtilities;

    [SetUp]
    public void Init()
    {
        containerTestUtilities = new ContainerTestUtilities();
        ItemContainers = new List<ItemContainer>();
        ItemDatas = new List<OwnedItemInformation>();
    }

    [Test]
    public void AddItem_BasicAdd_ReturnsStateAdd()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, container));
    }

    [Test]
    public void AddItem_RestrictedForContainer_ReturnsStateNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(true, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.AddItem(itemData, container));
    }

    [Test]
    public void AddItem_ItemLocked_ReturnsStateNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", true);

        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.AddItem(itemData, container));
    }

    [Test]
    public void MoveItem_RestrictedRemoveItem_ReturnsStateNo()
    {
        ItemContainer containerOne = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        ItemContainer containerTwo = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, containerOne));
        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.MoveItem(itemData, containerTwo));
    }

    [Test]
    public void MoveItem_ValidRemoveItemRestrictedAddItem_ReturnsStateNo()
    {
        ItemContainer containerOne = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemContainer containerTwo = containerTestUtilities.SetUpContainer(true, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, containerOne));
        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.MoveItem(itemData, containerTwo));
    }

    [Test]
    public void MoveItem_ValidRemoveItemValidAddItem_ReturnsStateAdd()
    {
        ItemContainer containerOne = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemContainer containerTwo = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, containerOne));
        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.MoveItem(itemData, containerTwo));
    }

    [Test]
    public void MoveItem_ItemLockedReturnsStateNo()
    {
        ItemContainer containerOne = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        ItemContainer containerTwo = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, containerOne));

        itemData.IsLocked = true;

        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.MoveItem(itemData, containerTwo));
    }

    [Test]
    public void RemoveItem_RestrictedRemoveItem_ReturnsStateNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, true, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, container));

        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.RemoveItem(itemData, container));
    }

    [Test]
    public void RemoveItem_ValidRemoveItem_ReturnsStateRemove()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, container));

        Assert.AreEqual(ContainerMoveState.ActionState.Remove, ItemContainerManager.RemoveItem(itemData, container));
    }

    [Test]
    public void RemoveItem_ItemLocked_ReturnsStateNo()
    {
        ItemContainer container = containerTestUtilities.SetUpContainer(false, false, ItemContainers);
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(10, 10, 10, 10, 10, 0, "Test Item", "123456", false);

        Assert.AreEqual(ContainerMoveState.ActionState.Add, ItemContainerManager.AddItem(itemData, container));

        itemData.IsLocked = true;

        Assert.AreEqual(ContainerMoveState.ActionState.No, ItemContainerManager.RemoveItem(itemData, container));
    }


    [TearDown]
    public void CleanUp()
    {
        ItemContainer[] objects = GameObject.FindObjectsOfType<ItemContainer>();

        foreach (ItemContainer container in objects)
        {
            DestroyImmediate(container.gameObject);
        }
    }

}
