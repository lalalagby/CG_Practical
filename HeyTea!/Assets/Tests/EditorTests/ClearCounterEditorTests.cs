using NUnit.Framework;
using UnityEngine;
using NSubstitute;

/*
File Name : SelectedCounterEditorTests.cs
Author    : Bingyu Guo
Created   : 10.05.2024
Last Modified by: Bingyu Guo
Last Modification Date  :   13.05.2024
*/
public class ClearCounterEditorTests
{
    private ClearCounter counter;
    private Player player;
    private HeyTeaObject playerHeyTeaObject;
    private HeyTeaObject counterHeyTeaObject;
    private HeyTeaObjectSO playerHeyTeaObjectSO;
    private HeyTeaObjectSO counterHeyTeaObjectSO;
    //private PotObject potObject;
    //private CupObject cupObject;

    [SetUp]
    public void Setup()
    {   
        counter = new GameObject().AddComponent<ClearCounter>();
        player = new GameObject().AddComponent<Player>();
        playerHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        counterHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        //potObject = new GameObject().AddComponent<PotObject>();
        //cupObject = new GameObject().AddComponent<CupObject>();
        playerHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        counterHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    [Test] // TC0101
    public void Interact_WhenCounterEmptyAndPlayerHasObject_PutObjectOnCounter()
    {
        // Arrange
        counter.SetHeyTeaObject(null);
        player.SetHeyTeaObject(playerHeyTeaObject);
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    [Test]  // TC0102
    public void Interact_WhenCounterHasObjectAndPlayerEmpty_TakeObjectToPlayer()
    {
        // Arrange
        player.SetHeyTeaObject(null);
        counter.SetHeyTeaObject(counterHeyTeaObject);
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
    }

    [Test]  // TC0103
    public void Interact_WhenCounterAndPlayerEmpty_NoChange()
    {
        // Arrange
        player.SetHeyTeaObject(null);
        counter.SetHeyTeaObject(null);
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    [Test]  // TC0104
    public void Interact_WhenBothCounterAndPlayerHoldContainer_NoChange()
    {
        // Arrange
        player.SetHeyTeaObject(playerHeyTeaObject);
        counter.SetHeyTeaObject(counterHeyTeaObject);

        playerHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.none;
        playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);
        counterHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.none;
        counterHeyTeaObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.AreEqual(playerHeyTeaObject, player.GetHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObject, counter.GetHeyTeaObject());
    }

    [Test]  // TC0105
    public void Interact_WhenBothCounterAndPlayerHoldFood_NoChange()
    {
        // Arrange
        player.SetHeyTeaObject(playerHeyTeaObject);
        counter.SetHeyTeaObject(counterHeyTeaObject);

        playerHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.teaBase;
        playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);
        counterHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.ingredients;
        counterHeyTeaObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.AreEqual(playerHeyTeaObject, player.GetHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObject, counter.GetHeyTeaObject());
    }

    //[Test]  // TC0106
    //public void Interact_WhenCounterHasContainerAndPlayerHasFood_PutFoodToCup()
    //{
    //    // Arrange
    //    player.SetHeyTeaObject(playerHeyTeaObject);
    //    counter.SetHeyTeaObject(potObject);

    //    playerHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.ingredients;
    //    playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);
    //    counterHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.unTreat;
    //    potObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

    //    // Act
    //    counter.Interact(player);

    //    // Assert
    //    Assert.IsTrue(counter.HasHeyTeaObject());
    //    Assert.IsFalse(player.HasHeyTeaObject());   // Bug
    //}

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(counter);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(playerHeyTeaObject);
        Object.DestroyImmediate(counterHeyTeaObject);
        Object.DestroyImmediate(playerHeyTeaObjectSO);
        Object.DestroyImmediate(counterHeyTeaObjectSO);
    }
}