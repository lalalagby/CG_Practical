using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Terrain;

/**
 * @author Bingyu Guo
 * 
 * @brief Unit tests for the ClearCounter class.
 * 
 * @details Class contains unit tests for various interaction scenarios involving the ClearCounter class,
 *          such as transferring HeyTeaObject between the Player and the ClearCounter, and ensuring correct state changes.
 * 
 * @note
 *      milkTeaMaterialType contains six types: teaBase, basicAdd, ingredients, fruit, none, un treat.
 *      HeyTeaObject includes the objects contained in teaBase, basicAdd, ingredients, fruit, none, un treat.
 *          - teaBase: tea, milk, milktea
 *          - basicAdd: sugar, ice
 *          - ingredients: red bean cooked, pearl cooked
 *          - fruit: orange slice, grape slice, strawberry slice
 *          - none: pot, cup
 *          - un treat: bagged pearl, bagged red bean, bagged sugar, grape, orange, strawberry
 */
public class ClearCounterEditorTests
{
    private ClearCounter counter;
    private Player player;
    private HeyTeaObject playerHeyTeaObject;
    private HeyTeaObject counterHeyTeaObject;
    private HeyTeaObjectSO playerHeyTeaObjectSO;
    private HeyTeaObjectSO counterHeyTeaObjectSO;

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the ClearCounter, Player, HeyTeaObject instances, and their corresponding ScriptableObjects.
     */
    [SetUp]
    public void Setup() {   
        counter = new GameObject().AddComponent<ClearCounter>();
        player = new GameObject().AddComponent<Player>();
        playerHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        counterHeyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        playerHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        counterHeyTeaObjectSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    /**
     * @brief [TC0101] Tests the scenario where the ClearCounter has nothing, but the Player has a HeyTeaObject(basicAdd, fruit, none, untreat).
     * 
     * @details This test case checks if the ClearCounter correctly takes the HeyTeaObject from the Player.
     * 
     * @param materialType The type of the HeyTeaObject held by the Player.
     */
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
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

    /**
     * @brief [TC0102] Tests the scenario where the ClearCounter has a HeyTeaObject, but the Player has nothing.
     * 
     * @details This test case checks if the Player correctly takes the HeyTeaObject from the ClearCounter.
     * 
     * @param materialType The type of the HeyTeaObject held by the ClearCounter.
     */
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.none)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
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

    /**
    * @brief [TC0103] Tests the scenario where both the ClearCounter and the Player have nothing.
    * 
    * @details This test case checks that nothing changes when both the ClearCounter and the Player have no HeyTeaObjects.
    */
    [Test]
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

    /**
     * @brief [TC0104] Tests the scenario where both the ClearCounter and the Player hold HeyTeaObjects.
     * 
     * @details This test case checks that nothing changes when both the ClearCounter and the Player have HeyTeaObjects(except pot or cup).
     * 
     * @param playerMaterialType The type of the HeyTeaObject held by the Player.
     * @param counterMaterialType The type of the HeyTeaObject held by the ClearCounter.
     */
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

    /**
     * @brief Tears down the test environment after each test.
     * 
     * @details This method destroys the ClearCounter, Player, and HeyTeaObject instances to clean up after each test.
     */
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