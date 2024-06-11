using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;

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
 * 
 *  When Player has a Cup, the interaction will be happened on Cup rather than Pot.
 */

public class PotObjectEditorTests {
    private Player player;
    private PotObject pot;
    private HeyTeaObjectSO ingredientSO;
    private HeyTeaObject ingredient;
    private HeyTeaObjectSO heyTeaObjectSO;

    public PotObject CreatePotObject() {
        var potPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Pot.prefab");
        Transform potInstance = Transform.Instantiate(potPrefab);

        return potInstance.GetComponent<PotObject>();
    }

    public CupObject CreateCupObject() {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path)  {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }


    [SetUp]
    public void Setup() {
        player = new GameObject().AddComponent<Player>();
        pot = CreatePotObject();
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    // [TC1001] Pot is empty, and Player has uncooked ingredient. Expect Pot has the uncooked ingredient from Player, Player has nothing.
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


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedSugar.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Ice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/OrangeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/GrapeSlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/StrawberrySlice.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Grape.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Orange.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Strawberry.asset")]
    // [TC1002] Pot is empty, Player has Fruit, basicAdd that cannot be added to the Pot. Nothing would be changed.
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

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    // [TC1003] Pot with cooked/uncooked Ingredient and Player holds uncooked Ingredients. Nothing would be changed.
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
