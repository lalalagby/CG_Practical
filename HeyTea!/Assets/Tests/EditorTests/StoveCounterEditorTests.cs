using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;


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

    /*
     *  Will have interaction between StoveCounter and Player in below tests.
     */

    
    [Test]
    // [TC0701] StoveCounter has Pot, but Player has nothing. Expect StoveCounter has nothing, but Player has Pot from StoveCounter.
    public void StoveCounterHasPot_PlayerHasNothing() {
        // Arrange
        pot = CreatePotObject();
        counter.SetHeyTeaObject(pot);
        player.ClearHeyTeaObject();
        
        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(counter.HasHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(pot, player.GetHeyTeaObject());
    }
   
    
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    // [TC0702] StoveCounter has Pot, and Player has uncooked Ingredients. Expect StoveCounter has Pot, Pot has Ingredients from Player, Player has nothing.
    public void StoveCounterHasPot_PlayerHasUncookedIngredients(string heyTeaObjectSOPath, string expectedHeyTeaObjectSOPath) {
        // Arrange
        counter.SetHeyTeaObject(pot);
        ingredientSO = CreateHeyTeaObjectSO(heyTeaObjectSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);
        ingredient.SetHeyTeaObjectParents(player);
        player.SetHeyTeaObject(ingredient);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsFalse(player.HasHeyTeaObject());
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(pot, counter.GetHeyTeaObject());

        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, CreateHeyTeaObjectSO(expectedHeyTeaObjectSOPath));
    }

    
    [Test]
    // [TC0703] StoveCounter has nothing, but Player has Pot. Expect StoveCounter has Pot from Player, but Player has nothing.
    public void StoveCounterHasNothing_PlayerHasPot() {
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


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    // [TC0704] StoveCounter has Pot with cooked Ingredient, Player has Cup with at most one differentingredient
    public void StoveCounterHasPotWithCookedIngredient_PlayerHasCupWhichCanBeAdded(string cupIngredientPath, string potIngredientPath) {
        // Arrange
        var cupIngredientSO = CreateHeyTeaObjectSO(cupIngredientPath);
        var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
        cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
        cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);
        player.SetHeyTeaObject(cup);
        

        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        counter.SetHeyTeaObject(pot);


        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), cup);

        var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == potIngredientSO), potIngredientSO);

        Assert.IsTrue(counter.GetHeyTeaObject());
        Assert.AreEqual(counter.GetHeyTeaObject(), pot);

        Assert.IsFalse(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    }




    /*
     *  Nothing would be changed in below tests. i.e. The state after the interaction is the same as before the interaction.
     */


    [Test]
    // [TC0705] StoveCounter and Player have nothing. 
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

    
    [Test]
    // [TC0706] StoveCounter has an empty Pot, and Player has Cup. 
    public void StoveCounterHasEmptyPot_PlayerHasCup() {
        // Arrange
        counter.SetHeyTeaObject(pot);
        player.SetHeyTeaObject(cup);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(pot, counter.GetHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(cup, player.GetHeyTeaObject());
    }

   
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.teaBase)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    // [TC0707] StoveCounter has Pot, and Player has other Food(i.e. not Ingredients).
    public void StoveCounterHasPot_PlayerHasOtherFood(HeyTeaObjectSO.MilkTeaMaterialType materialType) {
        // Arrange
        counter.SetHeyTeaObject(pot);
        player.SetHeyTeaObject(otherFood);
        otherFoodSO.materialType = materialType;
        otherFood.SetHeyTeaObjectSO(otherFoodSO);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(counter.HasHeyTeaObject());
        Assert.AreEqual(pot, counter.GetHeyTeaObject());
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(otherFood, player.GetHeyTeaObject());
        Assert.AreEqual(otherFoodSO.materialType, materialType);
    }

  
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.teaBase)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.ingredients)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    // [TC0708] StoveCounter has nothing, but Player has Food.
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

    
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    // [TC0709] StoveCounter has Pot with uncooked Ingredient, Player has Cup. 
    public void StoveCounterHasPotWithUncookedIngredient_PlayerHasCup(string uncookedIngredientSOPath)  {
        // Arrange
        player.SetHeyTeaObject(cup);

        ingredientSO = CreateHeyTeaObjectSO(uncookedIngredientSOPath);
        ingredient.SetHeyTeaObjectSO(ingredientSO);        
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.unTreat).AddHeyTeaObject(ingredient);
        counter.SetHeyTeaObject(pot);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), cup);

        Assert.IsTrue(counter.GetHeyTeaObject());
        Assert.AreEqual(counter.GetHeyTeaObject(), pot);

        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, ingredientSO);
    }


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    // [TC0710] StoveCounter has Pot with cooked Ingredient, Player has Cup with the same Ingredient.
    public void StoveCounterHasPotWithCookedIngredient_PlayerHasCupWithTheSameIngredient(string cookedIngredientPath) {
        // Arrange
        var cupIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
        var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
        cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
        cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);
        player.SetHeyTeaObject(cup);

        var potIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
        counter.SetHeyTeaObject(pot);

        // Act
        counter.Interact(player);

        // Assert
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(player.GetHeyTeaObject(), cup);

        var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);

        Assert.IsTrue(counter.GetHeyTeaObject());
        Assert.AreEqual(counter.GetHeyTeaObject(), pot);

        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, potIngredientSO);
    }

    
    [Test]
    // [TC0711] StoveCounter has nothing, but Player has Cup.
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
