using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

/**
 * @class SoupContainerCounterTests
 * @brief Unit tests for the SoupContainerCounter class, which handles interactions between the player and the soup container counter.
 * @author Xinyue Cheng
 * @details
 * The SoupContainerCounterTests class includes various test cases to validate the functionalities of the SoupContainerCounter class,
 * such as adding ingredients to kitchenware objects, and ensuring correct interactions based on the player's held objects.
 */
public class SoupContainerCounterTests
{
    private SoupContainerCounter soupContainerCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;
    private IKichenwareObejct kichenwareObject;
    private PotObject pot;
    private CupObject cup;

    /**
     * @brief Instantiates a prefab from a given path.
     * @param prefabPath The path of the prefab to instantiate.
     * @return The instantiated GameObject.
     */
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }

    /**
     * @brief Creates a HeyTeaObjectSO from a given path.
     * @param path The path of the HeyTeaObjectSO to create.
     * @return The created HeyTeaObjectSO.
     */
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Creates a PotObject from a predefined path.
     * @return The created PotObject.
     */
    private PotObject CreatePotObject()
    {
        var potPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Pot.prefab");
        Transform potInstance = Transform.Instantiate(potPrefab);

        return potInstance.GetComponent<PotObject>();
    }

    /**
     * @brief Creates a CupObject from a predefined path.
     * @return The created CupObject.
     */
    private CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Sets up the test environment before each test.
     */
    [SetUp]
    public void Setup()
    {
        player = new GameObject().AddComponent<Player>();
        soupContainerCounter = new GameObject().AddComponent<SoupContainerCounter>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        pot = CreatePotObject();
        cup = CreateCupObject();
    }

    /**
     * @brief [TC0901] Tests that the player cannot grab an object from the soup container counter when not holding any object.
     */
    [Test]
    public void PlayerHasNoObject_CannotGrabObjectFromSoupContainerCounter()
    {
        // Arrange
        player.ClearHeyTeaObject();

        // Act
        soupContainerCounter.Interact(player);

        // Assert
        LogAssert.Expect(LogType.Log, "Player is not holding any object.");
    }

    /**
     * @brief [TC0902] Tests that the player can add an ingredient to the cup when holding a cup.
     * @param containerCounterPath The path of the container counter prefab.
     * @param expectedSOPath The path of the expected HeyTeaObjectSO.
     */
    [TestCase("Assets/Prefabs/Counters/SoupCounter/MilkCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilk.asset")]
    [TestCase("Assets/Prefabs/Counters/SoupCounter/TeaCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset")]
    public void PlayerHasCup_CanAddIngredientToCup(string containerCounterPath, string expectedSOPath)
    {
        // Arrange
        player.ClearHeyTeaObject();
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        heyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.teaBase;
        player.SetHeyTeaObject(cup);
        soupContainerCounter = InstantiatePrefab(containerCounterPath).GetComponent<SoupContainerCounter>();
        // Act
        soupContainerCounter.Interact(player);

        // Assert
        //player has cup object
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), cup);

        //cup object has the right ingredient
        var outputIngredients = cup.GetOutputHeyTeaObejctSOList();
        Assert.IsTrue(outputIngredients.Contains(heyTeaObjectSO));
        LogAssert.Expect(LogType.Log, "Add ingredient to the cup.");
    }

    /**
     * @brief [TC0903] Tests that the player cannot add an ingredient to the pot when holding a pot.
     * @param containerCounterPath The path of the container counter prefab.
     * @param expectedSOPath The path of the expected HeyTeaObjectSO.
     */
    [TestCase("Assets/Prefabs/Counters/SoupCounter/MilkCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidMilk.asset")]
    [TestCase("Assets/Prefabs/Counters/SoupCounter/TeaCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/LiquidTea.asset")]
    public void PlayerHasPot_CannotAddIngredientToPot(string containerCounterPath, string expectedSOPath)
    {
        // Arrange
        player.ClearHeyTeaObject();
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);
        heyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.teaBase;
        player.SetHeyTeaObject(pot);
        soupContainerCounter = InstantiatePrefab(containerCounterPath).GetComponent<SoupContainerCounter>();
        // Act
        soupContainerCounter.Interact(player);

        // Assert
        //player has pot object
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), pot);

        //the ingredient cannot be added in pot object
        Assert.IsFalse(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    }

    /**
     * @brief [TC0904] Tests that the player cannot add an ingredient when holding a non-kitchenware object.
     * @param containerCounterPath The path of the container counter prefab.
     * @param expectedSOPath The path of the expected HeyTeaObjectSO.
     */
    [TestCase("Assets/Prefabs/Counters/SoupCounter/MilkCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/Prefabs/Counters/SoupCounter/TeaCounter.prefab", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    public void PlayerHasNonKitchenwareObject_CannotAddIngredient(string containerCounterPath, string expectedSOPath)
    {
        // Arrange
        player.ClearHeyTeaObject();
        heyTeaObjectSO = CreateHeyTeaObjectSO(expectedSOPath);
        heyTeaObject.SetHeyTeaObjectSO(heyTeaObjectSO);

        player.SetHeyTeaObject(heyTeaObject);

        soupContainerCounter = InstantiatePrefab(containerCounterPath).GetComponent<SoupContainerCounter>();
        // Act
        soupContainerCounter.Interact(player);

        // Assert
        //player has heytea object
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), heyTeaObject);

        LogAssert.Expect(LogType.Log, "Player is not holding a cup. Cannot interact with SoupContainerCounter.");
    }
}
