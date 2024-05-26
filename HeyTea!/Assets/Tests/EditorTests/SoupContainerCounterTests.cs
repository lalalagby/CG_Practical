using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static IKichenwareObejct;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SoupContainerCounterTests
{
    private SoupContainerCounter soupContainerCounter;
    private Player player;
    private HeyTeaObject heyTeaObject;
    private HeyTeaObjectSO heyTeaObjectSO;
    private IKichenwareObejct kichenwareObject;
    private PotObject pot;
    private CupObject cup;
    private GameObject InstantiatePrefab(string prefabPath)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return GameObject.Instantiate(prefab);
    }
    private HeyTeaObjectSO CreateHeyTeaObjectSO(string path)
    {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }
    private PotObject CreatePotObject()
    {
        var potPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Pot.prefab");
        Transform potInstance = Transform.Instantiate(potPrefab);

        return potInstance.GetComponent<PotObject>();
    }

    private CupObject CreateCupObject()
    {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    [SetUp]
    public void Setup()
    {
        player = new GameObject().AddComponent<Player>();
        soupContainerCounter = new GameObject().AddComponent<SoupContainerCounter>();
        heyTeaObject = new GameObject().AddComponent<HeyTeaObject>();
        pot = CreatePotObject();
        cup = CreateCupObject();

    }

    //player has no object, cannot grab object from soup container counter
    [Test]
    public void PlayerHasNoObject_CannotGrabObjectFromSoupContainerCounter()
    {
        // Arrange
        player.ClearHeyTeaObject();

        // Act
        soupContainerCounter.Interact(player);

        // Assert
        LogAssert.Expect(LogType.Log, "Cannot grab this object.");
    }

    //player has cup and can add ingredient to cup
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
        LogAssert.Expect(LogType.Log, "Add ingredient to the kitchenware object.");
    }

    //player has pot and cannot add ingredient to pot
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
        LogAssert.Expect(LogType.Log, "Cannot add ingredient to the kitchenware object.");

    }

    //player has no kitchenware object and cannot add ingredient
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
        Assert.AreEqual(player.GetHeyTeaObject(),heyTeaObject);

        LogAssert.Expect(LogType.Log, "Cannot put ingredient on SoupContainerCounter.");
    }

}
