using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static IKichenwareObejct;


/**
 * @author Bingyu Guo, Xinyue Cheng
 * 
 * @brief Unit tests for the CupObject class.
 * 
 * @details This class contains unit tests for various functionalities of the CupObject class,
 *          ensuring that ingredients are correctly transferred between the Cup and the Pot, 
 *          and validating the states of these objects.
 */
public class CupObjectEditorTests{
    private CupObject cup;
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
     * @brief Creates a CupObject instance from a prefab.
     * 
     * @details This method loads a CupObject prefab from the specified path and instantiates it.
     * 
     * @return A CupObject instance.
     */
    public CupObject CreateCupObject() {
        var cupPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/HeyTeaObjects/Cup.prefab");
        Transform cupInstance = Transform.Instantiate(cupPrefab);

        return cupInstance.GetComponent<CupObject>();
    }

    /**
     * @brief Creates a CupObject instance from a prefab.
     * 
     * @details This method loads a CupObject prefab from the specified path and instantiates it.
     * 
     * @return A CupObject instance.
     */
    public HeyTeaObjectSO CreateHeyTeaObjectSO(string path) {
        return AssetDatabase.LoadAssetAtPath<HeyTeaObjectSO>(path);
    }

    /**
     * @brief Sets up the test environment before each test.
     * 
     * @details This method initializes the CupObject and PotObject instances for testing.
     */
    [SetUp]
    public void Setup() {
        cup = CreateCupObject();
        pot = CreatePotObject();
    }

    /**
     * @brief [TC1101] Tests the scenario where the Cup and Pot have the same cooked ingredient.
     * 
     * @details This test case checks that nothing changes when both the Cup and Pot have the same cooked ingredient.
     * 
     * @param cookedIngredientPath The path to the cooked ingredient asset.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
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

    /**
     * @brief [TC1102] Tests the scenario where the Cup has at most one cooked ingredient, and the Pot has a different cooked ingredient.
     * 
     * @details This test case checks that the Cup correctly takes the cooked ingredient from the Pot,
     *          and the Pot is left empty.
     * 
     * @param cupIngredientPath The path to the cooked ingredient asset in the Cup.
     * @param potIngredientPath The path to the cooked ingredient asset in the Pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset", "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/PearlCooked.asset")]
    [TestCase(null, "Assets/ScriptableObjectSO/HeyTeaObjectSO/RedBeanCooked.asset")]
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

    /**
     * @brief [TC1103] Tests the scenario where the Cup is empty and the Pot has an uncooked ingredient.
     * 
     * @details This test case checks that nothing changes when the Cup is empty and the Pot has an uncooked ingredient.
     * 
     * @param uncookedIngredientSOPath The path to the uncooked ingredient asset in the Pot.
     */
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedPearl.asset")]
    [TestCase("Assets/ScriptableObjectSO/HeyTeaObjectSO/BaggedRedBean.asset")]
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

    /**
     * @brief [TC1104] Tests the scenario where both the Cup and Pot are empty.
     * 
     * @details This test case checks that nothing changes when both the Cup and Pot are empty.
     */
    [Test]
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
