using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;

public class CupObjectEditorTests{
    private CupObject cup;
    private PotObject pot;

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
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path) {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }


    [SetUp]
    public void Setup() {
        cup = CreateCupObject();
        pot = CreatePotObject();
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    // [TC1101] Cup has one cooked Ingredient, Pot has the same cooked Ingredient. Nothing would be changed.
    public void CupWithCookedIngredient_PotWithTheSameIngredient(string cookedIngredientPath) {
        // Arrange
        // Set Cup has one cooked Ingredient.
        var cupIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
        var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
        cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
        cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);

        // Set pot has the same cooked Ingredient.
        var potIngredientSO = CreateHeyTeaObjectSO(cookedIngredientPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);

        // Get heyTeaObjectSO from Pot. It should be potIngredientSO.
        pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);

        // Act
        // Try to add ingredient from Pot into Cup.
        if (heyTeaObjectSO && cup.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType))  {
            // Delete this HeyTeaObject from pot
            pot.DestroyChild(heyTeaObjectSO);
        }


        // Assert
        // Cup still has cupIngredientSO.
        var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);

        // Pot still has potIngredientSO.
        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, potIngredientSO);
    }


    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    // [TC1102] Cup has at most one cooked ingredient, Pot with one different cooked Ingredient. Expect Cup has the cooked Ingredient from Pot, and Pot has nothing.
    public void CupHasAtMostOneIngredient_PotHasDifferentCookedIngredient(string cupIngredientPath, string potIngredientPath) {
        // Arrange
        // Set Cup has one cooked Ingredient.
        var cupIngredientSO = CreateHeyTeaObjectSO(cupIngredientPath);
        var cupIngredient = new GameObject().AddComponent<HeyTeaObject>();
        cupIngredient.SetHeyTeaObjectSO(cupIngredientSO);
        cup.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(cupIngredient);

        // Set Pot has one different cooked Ingredient.
        var potIngredientSO = CreateHeyTeaObjectSO(potIngredientPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.ingredients).AddHeyTeaObject(potIngredient);

        // Get heyTeaObjectSO from Pot.It should be potIngredientSO.
        pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);

        // Act
        // Try to add ingredient from Pot into Cup.
        if (heyTeaObjectSO && cup.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType)) {
            // Delete this HeyTeaObject from pot
            pot.DestroyChild(heyTeaObjectSO);
        }

        // Assert
        //Cup has the cooked Ingredient from Pot.
        var heyTeaObjectSOList = cup.GetOutputHeyTeaObejctSOList();
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == cupIngredientSO), cupIngredientSO);
        Assert.AreEqual(heyTeaObjectSOList.FirstOrDefault(a => a == potIngredientSO), potIngredientSO);
        // Pot has nothing
        Assert.IsFalse(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
    }

    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
    // [TC1103] Cup is empty, Pot has one uncooked Ingredient. Nothing would be changed.
    public void EmptyCup_PotHasUncookedIngredient(string uncookedIngredientSOPath) {
        // Arrange
        // Set Pot has one uncooked Ingredient.
        var potIngredientSO = CreateHeyTeaObjectSO(uncookedIngredientSOPath);
        var potIngredient = new GameObject().AddComponent<HeyTeaObject>();
        potIngredient.SetHeyTeaObjectSO(potIngredientSO);
        pot.GetMilkTeaMaterialQuota().FirstOrDefault(a => a.milkTeaMaterialType is MilkTeaMaterialType.unTreat).AddHeyTeaObject(potIngredient);

        // Get heyTeaObjectSO from Pot.It should be potIngredientSO.
        pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);

        // Act
        // Try to add ingredient from Pot into Cup.
        if (heyTeaObjectSO && cup.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType)) {
            // Delete this HeyTeaObject from pot
            pot.DestroyChild(heyTeaObjectSO);
        }

        // Assert
        // Cup is still empty
        Assert.AreEqual(cup.GetOutputHeyTeaObejctSOList().Count, 0);
        // Pot still has the uncooked Ingredient 'potIngredientSO'.
        Assert.IsTrue(pot.GetOutputHeyTeaObejct(out heyTeaObjectSO));
        Assert.AreEqual(heyTeaObjectSO, potIngredientSO);
    }

    [Test]
    // [TC1104] Cup and Pot are both empty. Nothing would be changed.
    public void EmptyPot_EmptyCup()  {
        // Arrange
        // Get heyTeaObjectSO from Pot. It should be null.
        pot.GetOutputHeyTeaObejct(out HeyTeaObjectSO heyTeaObjectSO);

        // Act
        // Try to add ingredient from Pot into Cup.
        if(heyTeaObjectSO && cup.TryAddIngredient(heyTeaObjectSO, (MilkTeaMaterialType)heyTeaObjectSO.materialType)) {
            // Delete this HeyTeaObject from pot
            pot.DestroyChild(heyTeaObjectSO);
        }

        // Assert
        // Pot and Cup are both empty
        Assert.AreEqual(pot.CheckTotalInpotNum(), 0);
        Assert.AreEqual(cup.GetOutputHeyTeaObejctSOList().Count(), 0);
    }

}
