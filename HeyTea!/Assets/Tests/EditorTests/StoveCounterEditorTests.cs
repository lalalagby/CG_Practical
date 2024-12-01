using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;


/**
 * @author Bingyu Guo
 * 
 * @brief Unit tests for the StoveCounter class.
 * 
 * @details This class contains unit tests for various interaction scenarios involving the StoveCounter class,
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

public class StoveCounterEditorTests {
    private StoveCounter counter;
    private Player player;
    private PotObject pot;
    private CupObject cup;
    private HeyTeaObjectSO otherFoodSO;
    private HeyTeaObject otherFood;

    /**
     * @brief Creates a prefab instance from a specified path.
     * 
     * @details This method loads a prefab from the specified path and instantiates it.
     * 
     * @param path The path to the prefab asset.
     * @return A Transform instance of the prefab.
     */
    public Transform CreatePrefab(string path) {
        var ingredientPrefab = AssetDatabase.LoadAssetAtPath<Transform>(path);
        return Transform.Instantiate(ingredientPrefab);
    }

     /**
     * @brief Creates a PotObject instance from a prefab.
     * 
     * @details This method loads a PotObject prefab from the specified path and instantiates it.
     * 
     * @return A PotObject instance.
     */
    public PotObject CreatePotObject() {
        var potPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Pot.prefab");
        Transform potInstance = Transform.Instantiate(potPrefab);

        return potInstance.GetComponent<PotObject>();
    }

    /**
     * @brief Creates a CupObject instance from a prefab.
     * 
     * @details This method loads a CupObject prefab from the specified path and instantiates it.
     * 
     * @return A CupObject instance.
     */
    public CupObject CreateCupObject()  {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Creates a HeyTeaObjectSO instance from a specified path.
     * 
     * @details This method loads a HeyTeaObjectSO from the specified path.
     * 
     * @param path The path to the HeyTeaObjectSO asset.
     * @return A HeyTeaObjectSO instance.
     */
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path) {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the StoveCounter, Player, PotObject, and other required instances.
     */
    [SetUp]
    public void Setup() {
        counter = new GameObject().AddComponent<StoveCounter>();
        player = new GameObject().AddComponent<Player>();
        pot = CreatePotObject();
        cup = CreateCupObject();
        otherFood = new GameObject().AddComponent<HeyTeaObject>();
        otherFoodSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }


    /**
    * @brief [TC0701] Tests the scenario where the StoveCounter has an empty Pot, but the Player has nothing.
    * 
    * @details This test case checks if the Player correctly takes the Pot from the StoveCounter.
    */
    [Test]
    public void StoveCounterHasEmptyPot_PlayerHasNothing() {
        // Arrange
        //pot = CreatePotObject();
        counter.SetHeyTeaObject(pot);
        player.ClearHeyTeaObject();
        
        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(pot, player.GetHeyTeaObject());
    }

    /**
     * @brief [TC0702] Tests the scenario where the StoveCounter has a Pot with an Ingredient, but the Player has nothing.
     * 
     * @details This test case checks if the Player correctly takes the Pot with the ingredient from the StoveCounter.
     * 
     * @param potIngredientSOPath The path to the ingredient in the Pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/ReadBeanCooked.asset")]
    public void StoveCounterHasPotWithThing_PlayerHasNothing(string potIngredientSOPath) {
        // Arrange
        //pot = CreatePotObject();
        player.ClearHeyTeaObject();
        // Initial a cooked/uncooked Ingredient
        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientSOPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        // Add this Ingredient to Pot
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        // Set StoveCounter has Pot
        counter.SetHeyTeaObject(pot);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(pot, player.GetHeyTeaObject());
    }

    /**
     * @brief [TC0703] Tests the scenario where the StoveCounter has nothing, but the Player has an empty Pot.
     * 
     * @details This test case checks if the StoveCounter correctly takes the empty Pot from the Player.
     */
    [Test]
    public void StoveCounterHasNothing_PlayerHasEmptyPot() {
        // Arrange
        player.SetHeyTeaObject(pot);
        counter.ClearHeyTeaObject();

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(pot, counter.GetHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC0704] Tests the scenario where the StoveCounter has nothing, but the Player has a Pot with an Ingredient.
     * 
     * @details This test case checks if the StoveCounter correctly takes the Pot with the Ingredient from the Player.
     * 
     * @param potIngredientSOPath The path to the ingredient in the player's pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/ReadBeanCooked.asset")]
    public void StoveCounterHasNothing_PlayerHasPotWithThing(string potIngredientSOPath) {
        // Arrange
        counter.ClearHeyTeaObject();
        // Initial a cooked/uncooked Ingredient
        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientSOPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        // Add this Ingredient to Pot
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        // Set Player has Pot
        player.SetHeyTeaObject(pot);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(pot, counter.GetHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC0705] Tests the scenario where both the StoveCounter and the Player have nothing.
     * 
     * @details This test case checks that nothing changes when both the StoveCounter and the Player have no HeyTeaObjects.
     */
    [Test]
    public void StoveCounterAndPlayerHaveNothing() {
        // Arrange
        counter.ClearHeyTeaObject();
        player.ClearHeyTeaObject();

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC0706] Tests the scenario where the StoveCounter has nothing, but the Player has food.
     * 
     * @details This test case checks that nothing changes when the Player has a food object that cannot be added to the StoveCounter.
     * 
     * @param materialType The type of the food the Player has.
     */
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.teaBase)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.ingredients)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    // [TC0706] StoveCounter has nothing, but Player has Food. Nothing would be changed.
    public void StoveCounterHasNothing_PlayerHasFood(HeyTeaObjectSO.MilkTeaMaterialType materialType) {
        // Arrange
        counter.ClearHeyTeaObject();
        player.SetHeyTeaObject(otherFood);
        otherFoodSO.materialType = materialType;
        otherFood.SetHeyTeaObjectSO(otherFoodSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(otherFood, player.GetHeyTeaObject());
        Assert.AreEqual(otherFoodSO.materialType, materialType);
    }

    /**
     * @brief [TC0707] Tests the scenario where the StoveCounter has nothing, but the Player has a Cup.
     * 
     * @details This test case checks that nothing changes when the Player has a cup.
     */
    [Test]
    public void StoveCounterHasNothing_PlayerHasCup() {
        // Arrange
        counter.ClearHeyTeaObject();
        player.SetHeyTeaObject(cup);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(cup, player.GetHeyTeaObject());
    }
}
