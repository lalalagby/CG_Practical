using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NUnit.Framework;
using UnityEngine;
using static IKichenwareObejct;
using System.Linq;

/**
 * @brief Unit tests for the TrashCounter class.
 * 
 * @details This class contains unit tests to verify the functionality of the TrashCounter class, 
 *          ensuring that it interacts correctly with HeyTeaObjects and PotObjects held by the Player.
 */
public class TrashCounterEditorTests
{
    private TrashCounter trashCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private PotObject pot;

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
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path) {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the TrashCounter, Player, HeyTeaObject, and Pot instances.
     */
    [SetUp]
    public void Setup()
    {
        // Initialize TrashCounter
        trashCounter = new GameObject().AddComponent<TrashCounter>();

        // Initialize Player
        player = new GameObject().AddComponent<Player>();

        // Initialize HeyTeaObject and PotObject
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        pot = CreatePotObject();
    }

    /**
     * @brief [TC1201] Test to verify that the HeyTeaObject held by the Player is destroyed 
     *                  when interacting with the TrashCounter.
     * 
     * @param heyTeaObjectSOPath The path to the HeyTeaObjectSO asset.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Cup.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset")]
    // Player has a HeyTeaObject(Except Pot). Expect the HeyTeaObject will be destroied.
    public void PlayerHasHeyTeaObject_DestroysHeyTeaObject(string heyTeaObjectSOPath)
    {
        // Arrange
        // Initial a HeyTeaObject exceot Pot.
        var heyTeaObjectSO = CreateHeyTeaObjectSO(heyTeaObjectSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        heyTeaObject.SetHeyTeaObjectParents(player);
        // Set Player has HeyTeaObject.
        player.SetHeyTeaObject(heyTeaObject);

        // Act
        trashCounter.Interact(player);

        // Assert
        Assert.IsFalse(player.HasHeyTeaObject());
    }

    /**
     * @brief [TC1202] Test to verify that the Ingredient in the Pot held by the Player is destroyed 
     *                  when interacting with the TrashCounter.
     * 
     * @param potIngredientSOPath The path to the HeyTeaObjectSO asset representing the Ingredient in the Pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/ReadBeanCooked.asset")]
    public void PlayerHasPotObjectWithIngredient_DestroysIngredient(string potIngredientSOPath) {
        // Arrange
        // Initial a cooked/uncooked Ingredient
        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientSOPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        // Add this Ingredient to Pot
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        
        // Set Player holds the Pot
        player.SetHeyTeaObject(pot);

        // Act
        trashCounter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), pot);
        Assert.AreEqual(pot.CheckTotalInpotNum(), 0);
    }

    /**
     * @brief [TC1203] Test to verify that nothing happens when the Player has an empty Pot 
     *                  and interacts with the TrashCounter.
     */
    [Test]
    public void PlayerHasPotObject_NothingHappens()
    {
        // Arrange
        player.SetHeyTeaObject(pot);

        // Act
        trashCounter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), pot);
    }

    /**
     * @brief [TC1204] Test to verify that nothing happens when the Player has nothing 
     *                  and interacts with the TrashCounter.
     */
    [Test]
    public void PlayerHasNothing_NothingHappens()
    {
        // Arrange
        player.ClearHeyTeaObject();

        // Act
        trashCounter.Interact(player);

        // Assert
        Assert.IsFalse(player.HasHeyTeaObject());
    }
}
