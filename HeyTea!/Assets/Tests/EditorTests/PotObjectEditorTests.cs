using NUnit.Framework;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
 */

public class PotObjectEditorTests {
    private Player player;
    private PotObject pot;
    private CupObject cup;
    private HeyTeaObjectSO ingredientSO;
    private HeyTeaObjectSO otherFoodSO;
    private HeyTeaObject ingredient;
    private HeyTeaObject otherFood;
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
        cup = CreateCupObject(); 
        ingredient = new GameObject().AddComponent<HeyTeaObject>();
        otherFood = new GameObject().AddComponent<HeyTeaObject>();
        ingredientSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
        otherFoodSO = ScriptableObject.CreateInstance<HeyTeaObjectSO>();
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/Pearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBean.asset")]
    // [TC1001] Pot is empty, and Player has uncooked ingredient. Expect Pot has the uncooked ingredient from Player, Player has nothing.
    public void PotIsEmpty_PlayerHasUncookedIngredients(string heyTeaObjectSOPath, string expectedHeyTeaObjectSOPath)
    {
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


    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.unTreat)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.teaBase)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.fruit)]
    [TestCase(HeyTeaObjectSO.MilkTeaMaterialType.basicAdd)]
    // [TC1002] Pot is empty, Player has HeyTeaObject(except uncooked ingredients) that cannot be added to the Pot. Nothing would be changed.
    public void PotIsEmpty_PlayerHasHeyTeaObjectExceptUncookedIngredients(HeyTeaObjectSO.MilkTeaMaterialType materialType) {
        // Arrange
        player.SetHeyTeaObject(otherFood);
        otherFoodSO.materialType = materialType;
        otherFood.SetHeyTeaObjectSO(otherFoodSO);

        // Act
        pot.TryAddIngredient(player.GetHeyTeaObject().GetHeyTeaObjectSO(), (MilkTeaMaterialType)player.GetHeyTeaObject().GetHeyTeaObjectSO().materialType);

        // Assert
        Assert.AreEqual(pot.CheckTotalInpotNum(), 0);
        Assert.IsTrue(player.HasHeyTeaObject());
        Assert.AreEqual(otherFood, player.GetHeyTeaObject());
        Assert.AreEqual(otherFoodSO.materialType, materialType);
    }

    [Test]
    // [TC1003] Pot with cooked/uncooked Ingredient and Player holds uncooked Ingredients. Nothing would be changed.
    public void PotWithCookedOrUncookedIngredient_PlayerHasUncookedIngredient() {

    }




    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    //[TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    //[TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    //// [TC1003] Pot with cooked Ingredient, Player has Cup with at most one different ingredient. Expect Pot has nothing, and Cup has the cooked Ingredient from Pot.
    //public void PotWithCookedIngredient_PlayerHasCupWithAtMostOneDifferentIngredient(string cupIngredientPath, string potIngredientPath)  {
    //    // Arrange
    //    var cupIngredientSO = CreateHeyTeaObjectSO(cupIngredientPath);
    //    var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
    //    cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
    //    cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);

    //    var potIngredientSO = CreateHeyTeaObjectSO(potIngredientPath);
    //    var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
    //    potIngredient.SetHeyTeaObjectSO(potIngredientSO);
    //    pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
    //    var cupIkitchenwareObject = (IKichenwareObejct)cup;

    //    // Act
    //    cupIkitchenwareObject.InteractWithOtherKichenware(pot);
    //    cup = (CupObject)cupIkitchenwareObject;
    //    pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);
    //    pot.DestroyChild(heyTeaObjectSO);

    //    // Assert
    //    //Cup has the cooked Ingredient from Pot.
    //    var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
    //    Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);
    //    Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == potIngredientSO), potIngredientSO);
    //    // Pot has nothing
    //    Assert.IsFalse(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    //}


    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    //// [TC1004] Pot with cooked Ingredient, Player has Cup with the same Ingredient. Nothing would be changed.
    //public void PotWithCookedIngredient_PlayerHasCupWithTheSameIngredient(string cookedIngredientPath)
    //{
    //    // Arrange
    //    var cupIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
    //    var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
    //    cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
    //    cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);
    //    player.SetHeyTeaObject(cup);

    //    var potIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
    //    var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
    //    potIngredient.SetHeyTeaObjectSO(potIngredientSO);
    //    pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);
    //    counter.SetHeyTeaObject(pot);

    //    // Act
    //    counter.Interact(player);

    //    // Assert
    //    Assert.IsTrue(player.HasHeyTeaObject());
    //    Assert.AreEqual(player.GetHeyTeaObject(), cup);

    //    var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
    //    Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);

    //    Assert.IsTrue(counter.GetHeyTeaObject());
    //    Assert.AreEqual(counter.GetHeyTeaObject(), pot);

    //    Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    //    Assert.AreEqual(heyTeaObjectSO, potIngredientSO);
    //}


    //// [TC1005] Pot with one uncooked ingredient, and Player has HeyTeaObject. Nothing would be changed.

    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    //[TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    //// [TC0709] StoveCounter has Pot with uncooked Ingredient, Player has Cup. 
    //public void StoveCounterHasPotWithUncookedIngredient_PlayerHasCup(string uncookedIngredientSOPath)
    //{
    //    // Arrange
    //    player.SetHeyTeaObject(cup);

    //    ingredientSO = CreateHeyTeaObjectSO(uncookedIngredientSOPath);
    //    ingredient.SetHeyTeaObjectSO(ingredientSO);
    //    pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.unTreat).AddHeyTeaObject(ingredient);
    //    counter.SetHeyTeaObject(pot);

    //    // Act
    //    counter.Interact(player);

    //    // Assert
    //    Assert.IsTrue(player.HasHeyTeaObject());
    //    Assert.AreEqual(player.GetHeyTeaObject(), cup);

    //    Assert.IsTrue(counter.GetHeyTeaObject());
    //    Assert.AreEqual(counter.GetHeyTeaObject(), pot);

    //    Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    //    Assert.AreEqual(heyTeaObjectSO, ingredientSO);
    //}

    //[Test]
    //// [TC0706] StoveCounter has an empty Pot, and Player has Cup. 
    //public void StoveCounterHasEmptyPot_PlayerHasCup()
    //{
    //    // Arrange
    //    counter.SetHeyTeaObject(pot);
    //    player.SetHeyTeaObject(cup);

    //    // Act
    //    counter.Interact(player);

    //    // Assert
    //    Assert.IsTrue(counter.HasHeyTeaObject());
    //    Assert.AreEqual(pot, counter.GetHeyTeaObject());
    //    Assert.IsTrue(player.HasHeyTeaObject());
    //    Assert.AreEqual(cup, player.GetHeyTeaObject());
    //}








    //[Test]  // TC0104
    //public void Interact_WhenBothCounterAndPlayerHoldContainer_NoChange()
    //{
    //    // Arrange
    //    player.SetHeyTeaObject(playerHeyTeaObject);
    //    counter.SetHeyTeaObject(counterHeyTeaObject);

    //    playerHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.none;
    //    playerHeyTeaObject.SetHeyTeaObjectSO(playerHeyTeaObjectSO);
    //    counterHeyTeaObjectSO.materialType = HeyTeaObjectSO.MilkTeaMaterialType.none;
    //    counterHeyTeaObject.SetHeyTeaObjectSO(counterHeyTeaObjectSO);

    //    // Act
    //    counter.Interact(player);

    //    // Assert
    //    Assert.AreEqual(playerHeyTeaObject, player.GetHeyTeaObject());
    //    Assert.AreEqual(counterHeyTeaObject, counter.GetHeyTeaObject());
    //}
}
