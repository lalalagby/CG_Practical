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
 *  When StoveCounter has Pot, and Player has something, the interact will be happened on Pot or Cup.
 */

public class StoveCounterEditorTests {
    private StoveCounter counter;
    private Player player;
    private PotObject pot;
    private CupObject cup;
    private HeyTeaObjectSO ingredientSO;
    private HeyTeaObjectSO otherFoodSO;
    private HeyTeaObject ingredient;
    private HeyTeaObject otherFood;
    private HeyTeaObjectSO heyTeaObjectSO;

    public Transform CreatePrefab(string path) {
        var ingredientPrefab = AssetDatabase.LoadAssetAtPath<Transform>(path);
        return Transform.Instantiate(ingredientPrefab);
    }

    public PotObject CreatePotObject() {
        var potPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Pot.prefab");
        Transform potInstance = Transform.Instantiate(potPrefab);

        return potInstance.GetComponent<PotObject>();
    }

    public CupObject CreateCupObject()  {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path) {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }


    [SetUp]
    public void Setup() {
        counter = new GameObject().AddComponent<StoveCounter>();
        player = new GameObject().AddComponent<Player>();
        pot = CreatePotObject();
        cup = CreateCupObject();
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        otherFood = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        otherFoodSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    
    [Test]
    // [TC0701] StoveCounter has an empty Pot, but Player has nothing. Expect StoveCounter has nothing, but Player has Pot from StoveCounter.
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

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/ReadBeanCooked.asset")]
    // [TC0702] StoveCounter has Pot with cooked/uncooked Ingredient, but Player has nothing. Expect StoveCounter has nothing, but Player has Pot from StoveCounter.
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

    [Test]
    // [TC0703] StoveCounter has nothing, but Player has an Empty Pot. Expect StoveCounter has Pot from Player, but Player has nothing.
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


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/ReadBeanCooked.asset")]
    // [TC0704] StoveCounter has nothing, but Player has Pot  with cooked/uncooked Ingredient. Expect StoveCounter has Pot from Player, but Player has nothing.
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


    [Test]
    // [TC0703] StoveCounter and Player have nothing. Nothing would be changed. 
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
  
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.teaBase)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.ingredients)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    // [TC0704] StoveCounter has nothing, but Player has Food. Nothing would be changed.
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

    
    [Test]
    // [TC0705] StoveCounter has nothing, but Player has Cup. Nothing would be changed.
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
