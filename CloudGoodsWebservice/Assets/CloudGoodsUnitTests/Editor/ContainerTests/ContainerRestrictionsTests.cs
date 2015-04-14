using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container.Restrcitions;
using CloudGoods.SDK.Container;

[TestFixture]
public class ContainerRestrictionsTests : MonoBehaviour {

    ContainerTestUtilities containerTestUtilities;

    List<GameObject> objectsForCleanUp;

    [SetUp]
    public void Init()
    {
        containerTestUtilities = new ContainerTestUtilities();
        objectsForCleanUp = new List<GameObject>();
    }

    [Test]
    public void AddOnlyIsRescricted_AddingItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        AddOnlyRestriction restriction = restrictionObject.AddComponent<AddOnlyRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(false, restriction.IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void AddOnlyIsRestricted_RemovingItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        AddOnlyRestriction restriction = restrictionObject.AddComponent<AddOnlyRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(true, restriction.IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void RemoveOnlyIsRestricted_AddingItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);


        GameObject restrictionObject = new GameObject();
        RemoveOnlyRestriction restriction = restrictionObject.AddComponent<RemoveOnlyRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(true, restriction.IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void RemoveOnlyIsRetricted_RemovingItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        RemoveOnlyRestriction restriction = restrictionObject.AddComponent<RemoveOnlyRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(false, restriction.IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void NoActionIsRestricted_AddingItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        NoActionRestriction restriction = restrictionObject.AddComponent<NoActionRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(true, restriction.IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void NoAction_RemovingItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        NoActionRestriction restriction = restrictionObject.AddComponent<NoActionRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        Assert.AreEqual(true, restriction.IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ItemLimitIsRestricted_AddingAboveItemLimit_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        ItemLimitRestriction restriction = restrictionObject.AddComponent<ItemLimitRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        ItemContainer container = containerTestUtilities.SetUpContainer(false, false);
        restriction.RestrictedContainer = container;
        objectsForCleanUp.Add(container.gameObject);

        restriction.ContainerItemLimit = 0;

        Assert.AreEqual(true, restriction.IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ItemLimitIsRestricted_AddingBelowItemLimit_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);

        GameObject restrictionObject = new GameObject();
        ItemLimitRestriction restriction = restrictionObject.AddComponent<ItemLimitRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        ItemContainer container = containerTestUtilities.SetUpContainer(false, false);
        restriction.RestrictedContainer = container;
        objectsForCleanUp.Add(container.gameObject);

        restriction.ContainerItemLimit = 1;

        Assert.AreEqual(false, restriction.IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_AddingExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateClassIDRestriction(1 , true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_AddingNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateClassIDRestriction(10, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_AddingIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateClassIDRestriction(1, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_AddingNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateClassIDRestriction(1, true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_RemoveExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateClassIDRestriction(1, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_RemoveNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateClassIDRestriction(10, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_RemoveIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateClassIDRestriction(1, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ClassIDIsRestricted_RemoveNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateClassIDRestriction(10, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_AddingExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateCollectionsIDRestriction(1, true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_AddingNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateCollectionsIDRestriction(10, true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_AddingIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateCollectionsIDRestriction(1, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_AddingNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateCollectionsIDRestriction(10, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_RemoveExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateCollectionsIDRestriction(1, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_RemoveNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateCollectionsIDRestriction(10, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_RemoveIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateCollectionsIDRestriction(1, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void CollectionIDIsRestricted_RemoveNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateCollectionsIDRestriction(10, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_AddingExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateItemIDRestriction(1, true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_AddingNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateItemIDRestriction(10, true).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_AddingIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateItemIDRestriction(1, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_AddingNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateItemIDRestriction(10, false).IsRestricted(ContainerAction.add, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_RemoveExcludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateItemIDRestriction(1, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_RemoveNonExcludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateItemIDRestriction(10, true).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_RemoveIncludedItem_ReturnsFalse()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(false, CreateItemIDRestriction(1, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [Test]
    public void ItemIDIsRestricted_RemoveNonIncludedItem_ReturnsTrue()
    {
        OwnedItemInformation itemData = containerTestUtilities.CreateItemData(1, 1, 1, 10, 1, 0, "Test Item", "123456", false);
        Assert.AreEqual(true, CreateItemIDRestriction(10, false).IsRestricted(ContainerAction.remove, itemData));
    }

    [TearDown]
    public void FinishedTestCleanUp()
    {
        foreach (GameObject objDestroy in objectsForCleanUp)
        {
            DestroyImmediate(objDestroy);
        }
    }

    private ClassIDRestriction CreateClassIDRestriction(int classID, bool isExcluded)
    {
        GameObject restrictionObject = new GameObject();
        ClassIDRestriction restriction = restrictionObject.AddComponent<ClassIDRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        restriction.ClassIDList.Add(classID);
        restriction.IsExcluded = isExcluded;
        return restriction;
    }

    private CollectionsIDRestriction CreateCollectionsIDRestriction(int collectionsId, bool isExcluded)
    {
        GameObject restrictionObject = new GameObject();
        CollectionsIDRestriction restriction = restrictionObject.AddComponent<CollectionsIDRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        restriction.CollectionsIDList.Add(collectionsId);
        restriction.IsExcluded = isExcluded;
        return restriction;
    }

    private ItemIDRestriction CreateItemIDRestriction(int ItemId, bool isExcluded)
    {
        GameObject restrictionObject = new GameObject();
        ItemIDRestriction restriction = restrictionObject.AddComponent<ItemIDRestriction>();
        objectsForCleanUp.Add(restrictionObject);

        restriction.ItemIDList.Add(ItemId);
        restriction.IsExcluded = isExcluded;
        return restriction;
    }

}
