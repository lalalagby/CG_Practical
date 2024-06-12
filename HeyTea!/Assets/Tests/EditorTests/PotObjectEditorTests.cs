using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;

/**
 * @author Bingyu Guo
 * 
 * @brief Unit tests for the PotObject class.
 * 
 * @details This class contains unit tests for various interaction scenarios involving the PotObject class, 
 *          such as adding ingredients to the pot and ensuring correct state changes.
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

public class PotObjectEditorTests {
    private Player player;
    private PotObject pot;
    private HeyTeaObjectSO ingredientSO;
    private HeyTeaObject ingredient;
    private HeyTeaObjectSO heyTeaObjectSO;

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
     * @brief Creates a HeyTeaObjectSO instance from a specified path.
     * 
     * @details This method loads a HeyTeaObjectSO from the specified path.
     * 
     * @param path The path to the HeyTeaObjectSO asset.
     * @return A HeyTeaObjectSO instance.
     */
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path)  {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the Player and PotObject instances, and creates a new HeyTeaObject instance.
     */
    [SetUp]
    public void Setup() {
        player = new GameObject().AddComponent<Player>();
        pot = CreatePotObject();
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    /**
     * @brief [TC1001] Tests the scenario where the Pot is empty and the Player has an uncooked ingredient.
     * 
     * @details This test case checks if the Pot correctly accepts an uncooked ingredient from the Player and if the Player holds nothing.
     * 
     * @param heyTeaObjectSOPath The path to the Player's HeyTeaObjectSO asset.
     * @param expectedHeyTeaObjectSOPath The expected HeyTeaObjectSO asset path in the Pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    public void PotIsEmpty_PlayerHasUncookedIngredients(string heyTeaObjectSOPath, string expectedHeyTeaObjectSOPath) {
        // Arrange
        ingredientSO = CreateHeyTeaObjectSO(heyTeaObjectSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);
        ingredient.SetHeyTeaObjectParents(player);
        player.SetHeyTeaObject(ingredient);

        // Act
        pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType);
        player.GetHeyTeaObject().DestroySelf();
        player.ClearHeyTeaObject();

        // Assert
        // Player has nothing
        Assert.IsFalse(player.HasHeyTeaObject());
        // Pot has the uncooked ingredient from Player
        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, CreateHeyTeaObjectSO(expectedHeyTeaObjectSOPath));
    }

    /**
     * @brief [TC1002] Tests the scenario where the Pot is empty and the Player has a HeyTeaObject that cannot be added to the Pot.
     * 
     * @details This test case checks if nothing changes when the Pot is empty and 
     *          the Player has a HeyTeaObject that cannot be added(Fruit, basicAdd) to the Pot.
     * 
     * @param heyTeaObjectSOCannotBeAdded The path to the HeyTeaObjectSO asset that cannot be added to the pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    public void PotIsEmpty_PlayerHasHeyTeaObjectExceptUncookedIngredients(string heyTeaObjectSOCannotBeAdded) {
        // Arrange. Inital a HeyTeaObject which cannot be added into Pot
        var heyTeaObjectSO = CreateHeyTeaObjectSO(heyTeaObjectSOCannotBeAdded);
        var heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        // Set Player holds this HeyTeaObject
        player.SetHeyTeaObject(heyTeaObject);

        // Act
        pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType);

        // Assert
        Assert.AreEqual(pot.CheckTotalInpotNum(), 0);
        Assert.AreEqual(heyTeaObject, player.GetHeyTeaObject());
    }

    /**
     * @brief [TC1003] Tests the scenario where the Pot has a cooked or uncooked ingredient and the Player has an uncooked ingredient.
     * 
     * @details This test case checks if nothing changes when the Pot already has an ingredient and the Player attempts to add another uncooked ingredient.
     * 
     * @param potIngredientSOPath The path to the ingredient in the Pot.
     * @param playerUncookedIngredientSOPath The path to the uncooked ingredient held by the Player.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    public void PotWithCookedOrUncookedIngredient_PlayerHasUncookedIngredient(string potIngredientSOPath, string playerUncookedIngredientSOPath) {
        // Arrange

        // Initial a cooked/uncooked Ingredient
        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientSOPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        // Add this Ingredient to Pot
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        //Initial an uncooked Ingredient which player holds
        var playerUncookedIngredientSO = CreateHeyTeaObjectSO(playerUncookedIngredientSOPath);
        var playerUncookedIngredient = new GameObject().AddComponent<HeyTeaObject>();
        playerUncookedIngredient.SetHeyTeaObjectSO(playerUncookedIngredientSO);
        player.SetHeyTeaObject(playerUncookedIngredient);

        // Act
        pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType);

        // Assert
        //Assert.AreEqual(pot.GetHeyTeaObjectSO(), potIngredientSO);
        Assert.AreEqual(player.GetHeyTeaObject(), playerUncookedIngredient);
    }
}
