using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Terrain;

/*
File Name : SelectedCounterEditorTests.cs
Author    : Bingyu Guo
Created   : 10.05.2024
Last Modified by: Bingyu Guo
Last Modification Date  :   13.05.2024
*/


/*
 * Glossary
 * 
 * HeyTeaObject: teaBase, basicAdd, ingredients, fruit, none, un treat
 *      - teaBase: tea, milk, milktea
 *      - basicAdd: sugar, ice
 *      - ingredients: red bean cooked, pearl cooked
 *      - fruit: orange slice, grape slice, strawberry slice
 *      - none: pot, cup
 *      - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry, 
 */
public class ClearCounterEditorTests
{
    private ClearCounter counter;
    private Player player;
    private HeyTeaObject playerHeyTeaObject;
    private HeyTeaObject counterHeyTeaObject;
    private HeyTeaObjectSO playerHeyTeaObjectSO;
    private HeyTeaObjectSO counterHeyTeaObjectSO;

    [SetUp]
    public void Setup() {   
        counter = new GameObject().AddComponent<ClearCounter>();
        player = new GameObject().AddComponent<Player>();
        playerHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        counterHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        playerHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        counterHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    // [TC0101] When ClearCounter has nothing, and Player has an HeyTeaObject(basicAdd, fruit, none, untreat). Expect to put the thing from Player to the ClearCounter.
    public void ClearCounterHasNothing_PlayerHasHeyTeaObject(HeyTeaObjectSO.MilkTeaMaterialType materialType) {
        // Arrange
        counter.ClearHeyTeaObject();
        player.SetHeyTeaObject(playerHeyTeaObject);
        playerHeyTeaObjectSO.materialType = materialType;
        playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(player.HasHeyTeaObject());
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(playerHeyTeaObject, counter.GetHeyTeaObject());
        Assert.AreEqual(playerHeyTeaObjectSO.materialType, materialType);
    }

    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    // [TC0102] When ClearCounter has something, and Player has nothing. Expect to put the thing from ClearCounter to Player.
    public void ClearCounterHasHeyTeaObject_PlayerHasNothing(HeyTeaObjectSO.MilkTeaMaterialType materialType)
    {
        // Arrange
        player.ClearHeyTeaObject();
        counter.SetHeyTeaObject(counterHeyTeaObject);
        counterHeyTeaObjectSO.materialType = materialType;
        counterHeyTeaObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObject, player.GetHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObjectSO.materialType, materialType);
    }

    [Test]
    // [TC0103] When ClearCounter has nothing, and Player has nothing. Nothing would be changed.
    public void ClearCounter_PlayerHasNothing() {
        // Arrange
        player.ClearHeyTeaObject();
        counter.ClearHeyTeaObject();

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd, HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd, HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd, HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd, HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit, HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit, HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit, HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit, HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none, HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none, HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none, HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none, HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat, HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat, HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat, HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat, HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    // [TC0104] When ClearCounter has HeyTeaObject(except pot or cup), and Player has HeyTeaObject(except pot or cup). Nothing would be changed.
    public void BothClearCounterAndPlayerHoldFood(HeyTeaObjectSO.MilkTeaMaterialType playerMaterialType, HeyTeaObjectSO.MilkTeaMaterialType counterMaterialType) {
        // Arrange
        player.SetHeyTeaObject(playerHeyTeaObject);
        playerHeyTeaObjectSO.materialType = playerMaterialType;
        playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);
        counter.SetHeyTeaObject(counterHeyTeaObject);
        counterHeyTeaObjectSO.materialType = counterMaterialType;
        counterHeyTeaObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(playerHeyTeaObject, player.GetHeyTeaObject());
        Assert.AreEqual(playerHeyTeaObjectSO.materialType, playerMaterialType);

        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObject, counter.GetHeyTeaObject());
        Assert.AreEqual(counterHeyTeaObjectSO.materialType, counterMaterialType);
    }


    [TearDown]
    public void TearDown() {
        Object.DestroyImmediate(counter);
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(playerHeyTeaObject);
        Object.DestroyImmediate(counterHeyTeaObject);
        Object.DestroyImmediate(playerHeyTeaObjectSO);
        Object.DestroyImmediate(counterHeyTeaObjectSO);
    }
}